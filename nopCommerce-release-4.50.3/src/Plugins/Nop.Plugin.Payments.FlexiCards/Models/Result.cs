using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class Result
    {
        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }
        [JsonProperty("resultDesc")]
        public string ResultDesc { get; set; }
    }
}
