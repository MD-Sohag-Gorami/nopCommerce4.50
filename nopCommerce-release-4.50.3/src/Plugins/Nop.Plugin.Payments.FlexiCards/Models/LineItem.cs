using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class LineItem
    {
        [JsonProperty("merchant_product_code")]
        public string MerchantProductCode { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
