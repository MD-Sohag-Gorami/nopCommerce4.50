using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.FlexiCards.Factories;
using Nop.Plugin.Payments.FlexiCards.Models;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Payments.FlexiCards.Controllers
{
    public class FlexiCardsPaymentController : BaseAdminController
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService; 
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;
        private readonly IFlexiCardsPaymentFactory _flexiCardsPaymentFactory;
        #endregion
        #region Ctor

        public FlexiCardsPaymentController(IOrderService orderService, IStoreContext storeContext, 
                                            ISettingService settingService, 
                                            IOrderProcessingService orderProcessingService, ILogger logger,
                                            IFlexiCardsPaymentFactory flexiCardsPaymentFactory)
        {
            _orderService = orderService;
            _storeContext = storeContext;
            _settingService = settingService;
            _orderProcessingService = orderProcessingService;
            _logger = logger;
            _flexiCardsPaymentFactory = flexiCardsPaymentFactory;
        }
        #endregion

        public async Task<IActionResult> PostPaymentHandler(string merchant_transaction_id, string process_no)
        {
            var success = int.TryParse(merchant_transaction_id, out var orderId);
            if (!success)
            {
                await _logger.ErrorAsync("Invalid Order Id(merchant_transaction_id)");
                return RedirectToRoute("HomePage");
            }

            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                await _logger.ErrorAsync($"No order found with order Id{orderId}");
                return RedirectToRoute("HomePage");
            }
            var storeScope = (await _storeContext.GetCurrentStoreAsync()).Id;
            var settings = await _settingService.LoadSettingAsync<FlexiCardsPaymentSettings>(storeScope);

            var requestUrl = settings.UseSandbox ? FlexiCardsPaymentDeafults.PaymentStatusTestEndPoint : FlexiCardsPaymentDeafults.PaymentStatusEndPoint;
            var requestBody = new PaymentStatusRequest()
            {
                MerchantId = settings.MerchantId,
                LoginId = settings.LoginId,
                Password = settings.Password,
                ApiKey = settings.ApiKey,
                ProcessNo = process_no,
                MerchantTransactionId = merchant_transaction_id,
                TransmissionDateTime = DateTime.UtcNow.ToString("yyyyMMddhhmmss")
            };
            requestUrl = _flexiCardsPaymentFactory.PrepareFlexiCardsPaymentStatusQueryString(requestUrl, requestBody);

            var request = WebRequest.Create(requestUrl);
            request.Method = "GET";
            try
            {
                var webResponse = request.GetResponse();
                using var webStream = webResponse.GetResponseStream() ?? Stream.Null;
                using var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                var paymentStatusResponse = JsonConvert.DeserializeObject<PaymentStatusResponse>(response);

                if (paymentStatusResponse.PaymentStatus.StatusCode == FlexiCardsPaymentDeafults.PaymentStatusApproved)
                {
                    await _orderService.InsertOrderNoteAsync(new OrderNote()
                    {
                        OrderId = order.Id,
                        Note = $"Payment Completed with transaction Id:{process_no}",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    order.CaptureTransactionId = paymentStatusResponse.RetrievalReferenceNumber;
                    order.CaptureTransactionResult = "Success";
                    await _orderService.UpdateOrderAsync(order);
                    await _orderProcessingService.MarkOrderAsPaidAsync(order);

                    return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
                }
                else
                {
                    var errorNote = $"Payment not approved with payment status code{paymentStatusResponse.PaymentStatus.StatusCode}";
                    await _logger.ErrorAsync(errorNote);
                    await _orderService.InsertOrderNoteAsync(new OrderNote()
                    {
                        OrderId = order.Id,
                        Note = errorNote,
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    return RedirectToRoute("OrderDetails", new { orderId = order.Id });
                    //order details redirection
                }
            }
            catch (Exception ex)
            {
                await _orderService.InsertOrderNoteAsync(new OrderNote()
                {
                    OrderId = order.Id,
                    Note = "Flexi Cards Payment - Web Response failed",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                //await _logger.ErrorAsync("Flexi Cards Payment - Web Response failed", ex);
                await _logger.InsertLogAsync(Core.Domain.Logging.LogLevel.Error, "FlexiCards Payment - Web Response failed", ex.Message);
                return RedirectToRoute("OrderDetails", new { orderId = order.Id });
            }
        }
    }
}
