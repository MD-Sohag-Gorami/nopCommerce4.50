using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.FlexiCards.Areas.Admin.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Payments.FlexiCards.Areas.Admin.Controllers
{
    public class FlexiCardsController : BaseAdminController
    {
        #region Fields
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        #endregion
        #region Ctor
        public FlexiCardsController(IStoreContext storeContext,
                                    ISettingService settingService,
                                    INotificationService notificationService,
                                    ILocalizationService localizationService)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> Configure()
        {

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<FlexiCardsPaymentSettings>(storeScope);

            var model = new FlexiCardsPaymentConfigureModel()
            {
                UseSandbox = settings.UseSandbox,
                MerchantId = settings.MerchantId,
                LoginId = settings.LoginId,
                Password = settings.Password,
                ApiKey = settings.ApiKey,
                AdditionalFee = settings.AdditionalFee,
                AdditionalFeePercentage = settings.AdditionalFeePercentage,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.UseSandbox_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.UseSandbox, storeScope);
                model.MerchantId_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.MerchantId, storeScope);
                model.LoginId_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.LoginId, storeScope);
                model.Password_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.Password, storeScope);
                model.ApiKey_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.ApiKey, storeScope);
                model.AdditionalFee_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.AdditionalFeePercentage, storeScope);
            }

            return View("~/Plugins/Payment.FlexiCards/Areas/Admin/Views/FlexiCards/Configure.cshtml",model);
        }
        [HttpPost]
        public async Task<IActionResult> Configure(FlexiCardsPaymentConfigureModel model)
        {
            if (ModelState.IsValid)
            {
                var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
                var settings = await _settingService.LoadSettingAsync<FlexiCardsPaymentSettings>(storeScope);

                settings.UseSandbox = model.UseSandbox;
                settings.MerchantId = model.MerchantId;
                settings.LoginId = model.LoginId;
                settings.Password = model.Password;
                settings.ApiKey = model.ApiKey;
                settings.AdditionalFee = model.AdditionalFee;
                settings.AdditionalFeePercentage = model.AdditionalFeePercentage;

                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.MerchantId, model.MerchantId_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.LoginId, model.LoginId_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.ApiKey, model.ApiKey_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
                await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);

                await _settingService.ClearCacheAsync();

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            }
            return View("~/Plugins/Payment.FlexiCards/Areas/Admin/Views/FlexiCards/Configure.cshtml", model);
        }
        #endregion
    }
}
