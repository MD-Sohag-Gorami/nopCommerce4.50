using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.FlexiCards.Factories;
using Nop.Plugin.Payments.FlexiCards.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace Nop.Plugin.Payments.FlexiCards
{
    public class FlexiCardsPaymentPlugin : BasePlugin, IPaymentMethod
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly IStoreContext _storeContext;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlexiCardsPaymentSettings _flexiCardsPaymentSettings;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IFlexiCardsPaymentFactory _flexiCardsPaymentFactory;
        #endregion
        #region Ctor
        public FlexiCardsPaymentPlugin(ISettingService settingService,
                                ILocalizationService localizationService,
                                IWebHelper webHelper,
                                IStoreContext storeContext,
                                IOrderService orderService,
                                IHttpContextAccessor httpContextAccessor,
                                FlexiCardsPaymentSettings flexiCardsPaymentSettings,
                                IOrderTotalCalculationService orderTotalCalculationService,
                                IFlexiCardsPaymentFactory flexiCardsPaymentFactory)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _storeContext = storeContext;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _flexiCardsPaymentSettings = flexiCardsPaymentSettings;
            _orderTotalCalculationService = orderTotalCalculationService;
            _flexiCardsPaymentFactory = flexiCardsPaymentFactory;
        }
        #endregion
        public bool SupportCapture => true;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => false;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;


        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
                return Task.FromResult(new CancelRecurringPaymentResult());
        }

        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            return Task.FromResult(true);
        }

        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            return Task.FromResult(new CapturePaymentResult { Errors = new[] { "Capture method not supported" } });

        }

        public async Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return await _orderTotalCalculationService.CalculatePaymentAdditionalFeeAsync(cart,
                _flexiCardsPaymentSettings.AdditionalFee, _flexiCardsPaymentSettings.AdditionalFeePercentage);
        }

        public string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/FlexiCards/Configure";
        }

        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest());
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await _localizationService.GetResourceAsync("Plugins.Payments.FlexiCardsPayment.PaymentMethodDescription");

        }

        public string GetPublicViewComponentName()
        {
            return "FlexiCards";
            ;
        }

        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(false);
        }

        public async override Task InstallAsync()
        {
            var settings = new FlexiCardsPaymentSettings()
            {
                MerchantId = "12321",
                LoginId = "203399",
                Password = "Password01",
                ApiKey = "2bef9740cd0be5995e58f9eef3249cabbd65d4bc",
                UseSandbox = true
            };
            await _settingService.SaveSettingAsync(settings);
           
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Payments.FlexiCards.Fields.UseSandbox"] = "Use Sandbox",
                ["Plugins.Payments.FlexiCards.Fields.UseSandbox.Hint"] = "Check to enable Sandbox(testing environment)",
                ["Plugins.Payments.FlexiCards.Fields.MerchantId"] = "Merchant ID",
                ["Plugins.Payments.FlexiCards.Fields.MerchantId.Hint"] = "Your merchant id provided to you by Flexi Cards",
                ["Plugins.Payments.FlexiCards.Fields.LoginId"] = "Login ID",
                ["Plugins.Payments.FlexiCards.Fields.LoginId.Hint"] = "Your login id provided to you by Flexi Cards",
                ["Plugins.Payments.FlexiCards.Fields.Password"] = "Password",
                ["Plugins.Payments.FlexiCards.Fields.Password.Hint"] = "Your password provided to you by Flexi Cards",
                ["Plugins.Payments.FlexiCards.Fields.ApiKey"] = "API Key",
                ["Plugins.Payments.FlexiCards.Fields.ApiKey.Hint"] = "Key defined by Flexi Cards for this service",
                ["Plugins.Payments.FlexiCards.Fields.AdditionalFee"] = "Additional fee",
                ["Plugins.Payments.FlexiCards.Fields.AdditionalFee.Hint"] = "Enter additional fee to charge your customers.",
                ["Plugins.Payments.FlexiCards.Fields.AdditionalFeePercentage"] = "Additional fee. Use percentage",
                ["Plugins.Payments.FlexiCards.Fields.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
                ["Plugins.Payments.FlexiCards.PaymentMethodDescription"] = "Pay with flexi  Cards",
            });
            await base.InstallAsync();
        }

        public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var storeScope = (await _storeContext.GetCurrentStoreAsync()).Id;
            var settings = await _settingService.LoadSettingAsync<FlexiCardsPaymentSettings>(storeScope);

            var requestUrl = settings.UseSandbox ? FlexiCardsPaymentDeafults.PaymentUrlTestEndPoint : FlexiCardsPaymentDeafults.PaymentUrlEndPoint;

            var request = WebRequest.Create(requestUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            var order = postProcessPaymentRequest.Order;
            var requestBody = await _flexiCardsPaymentFactory.PreparePaymentUrlRequest(settings, order);

            var json = JsonConvert.SerializeObject(requestBody);
            request.ContentLength = json.Length;
            using (var webStream = request.GetRequestStream())
            {
                using var requestWriter = new StreamWriter(webStream, Encoding.ASCII);
                requestWriter.Write(json);
            }
            try
            {
                var webResponse = request.GetResponse();
                using var webStream = webResponse.GetResponseStream() ?? Stream.Null;
                using var responseReader = new StreamReader(webStream);
                var response = responseReader.ReadToEnd();
                var paymentUrlResponse = JsonConvert.DeserializeObject<PaymentUrlResponse>(response);

                if (paymentUrlResponse.Result.ResultCode == FlexiCardsPaymentDeafults.ResponseSuccessful)
                {
                    var oderId = paymentUrlResponse.MerchantTransactionId;
                    var processNo = paymentUrlResponse.ProcessNo;
                    var redirectionUrl = paymentUrlResponse.PaymentUrl;

                    order.AuthorizationTransactionId = processNo;
                    await _orderService.UpdateOrderAsync(order);

                    _httpContextAccessor.HttpContext.Response.Redirect(redirectionUrl);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Task PreparePluginToUninstallAsync()
        {
            return Task.FromResult(new ProcessPaymentResult());
        }

        public Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult());

        }

        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult());

        }

        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Refund method not supported" } });

        }

        public async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<FlexiCardsPaymentSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.FlexiCards");

            await base.UninstallAsync();
        }

        public Task UpdateAsync(string currentVersion, string targetVersion)
        {
            return Task.FromResult(new ProcessPaymentResult());
        }

        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Void method not supported" } });

        }
    }
}
