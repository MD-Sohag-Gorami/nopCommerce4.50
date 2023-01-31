using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class PaymentStatus
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
        [JsonProperty("statusDesc")]
        public string StatusDesc { get; set; }
    }
}
