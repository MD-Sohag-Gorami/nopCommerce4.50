using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.FlexiCards.Areas.Admin.Models
{
    public record FlexiCardsPaymentConfigureModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandbox_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.MerchantId")]
        public string MerchantId { get; set; }
        public bool MerchantId_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.LoginId")]
        public string LoginId { get; set; }
        public bool LoginId_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.ApiKey")]
        public string ApiKey { get; set; }
        public bool ApiKey_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.FlexiCards.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }
    }
}
