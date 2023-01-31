using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
        public class ProductCodes
        {
            [JsonProperty("product_code")]
            public List<string> ProductCode { get; set; }
        }
}
