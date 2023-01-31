using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class PaymentUrlResponse
    {
        [JsonProperty("payment_url")]
        public string PaymentUrl { get; set; }
        [JsonProperty("process_no")]
        public string ProcessNo { get; set; }
        [JsonProperty("merchant_transaction_id")]
        public string MerchantTransactionId { get; set; }
        [JsonProperty("result")]
        public Result Result { get; set; }
        [JsonProperty("transmission_date_time")]
        public string TransmissionDateTime { get; set; }
    }
}
