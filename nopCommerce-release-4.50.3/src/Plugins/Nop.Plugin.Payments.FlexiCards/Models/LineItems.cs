using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class LineItems
    {
        [JsonProperty("lineItem")]
        public List<LineItem> LineItem { get; set; }
    }
}
