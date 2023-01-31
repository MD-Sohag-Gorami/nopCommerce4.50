using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class PaymentStatusRequest
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }
        [JsonProperty("login_id")]
        public string LoginId { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("process_no")]
        public string ProcessNo { get; set; }
        [JsonProperty("merchant_transaction_id")]
        public string MerchantTransactionId { get; set; }
        [JsonProperty("transmission_date_time")]
        public string TransmissionDateTime { get; set; }
    }
}
