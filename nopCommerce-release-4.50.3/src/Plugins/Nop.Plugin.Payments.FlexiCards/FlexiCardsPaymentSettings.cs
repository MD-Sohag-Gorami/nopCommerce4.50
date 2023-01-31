using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.FlexiCards
{
    public class FlexiCardsPaymentSettings : ISettings
    {
        public string MerchantId { get; set; }
        public string LoginId { get; set; }
        public string ApiKey { get; set; }
        public string Password { get; set; }
        public bool UseSandbox { get; set; }
        public bool AdditionalFeePercentage { get; set; }
        public decimal AdditionalFee { get; set; }
    }
}
