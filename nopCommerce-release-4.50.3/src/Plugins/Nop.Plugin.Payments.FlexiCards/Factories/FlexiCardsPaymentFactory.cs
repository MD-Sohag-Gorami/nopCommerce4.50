using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.FlexiCards.Models;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Payments.FlexiCards.Factories
{
    public class FlexiCardsPaymentFactory : IFlexiCardsPaymentFactory
    {
        private readonly IWebHelper _webHelper;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public FlexiCardsPaymentFactory(IWebHelper webHelper, IOrderService orderService, IProductService productService)
        {
            _webHelper = webHelper;
            _orderService = orderService;
            _productService = productService;
        }
        public async Task<PaymentUrlRequest> PreparePaymentUrlRequest(FlexiCardsPaymentSettings settings, Order order)
        {
            var lineItems = await (await _orderService.GetOrderItemsAsync(order.Id)).SelectAwait(async x =>
            {
                var product = await _productService.GetProductByIdAsync(x.ProductId);
                var lineItem = new LineItem()
                {
                    MerchantProductCode = product.Sku,
                    Description = product.Name,
                    Quantity = x.Quantity,
                    Amount = (int)(product.Price * 100)
                };
                return lineItem;
            }).ToListAsync();

            var requestBody = new PaymentUrlRequest()
            {
                MerchantId = settings.MerchantId,
                LoginId = settings.LoginId,
                Password = settings.Password,
                ApiKey = settings.ApiKey,
                MerchantTransactionId = order.Id.ToString(),
                TransactionAmount = Decimal.ToInt32(order.OrderTotal * 100),
                IncludeProductCodes = new ProductCodes()
                {
                    ProductCode = PrepareFlexiCardsProducts()
                },
                ExcludeProductCodes = new ProductCodes(),
                UrlResponse = $"{_webHelper.GetStoreLocation()}FlexiCardsPayment/PostPaymentHandler",
                DirectToUrlResponse = true,
                LineItems = new LineItems()
                {
                    LineItem = lineItems
                },
                TransmissionDateTime = DateTime.UtcNow.ToString("yyyyMMddhhmmss")
            };

            return requestBody;
        }

        public List<string> PrepareFlexiCardsProducts()
        {
            var qCardProducts = new List<string>()
            {
                "Post payment method test",
                "Post payment method identify"
            };
            return qCardProducts;
        }
        public string PrepareFlexiCardsPaymentStatusQueryString(string requestUrl, PaymentStatusRequest requestBody)
        {
            string temp = "?";
            int i = 0;

            var propertyInfos = requestBody.GetType().GetProperties();
            foreach (PropertyInfo prop in propertyInfos)
            {
                if (i > 0)
                {
                    temp += "&";
                }
                var jsonPropertyName = prop.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;

                temp += (jsonPropertyName + "=" + prop.GetValue(requestBody, null));
                i++;
            }
            return requestUrl + temp;
        }
    }
}
