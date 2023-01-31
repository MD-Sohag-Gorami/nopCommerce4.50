using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Nop.Core.Domain.Payments;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class PaymentStatusResponse
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
        [JsonProperty("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }
        [JsonProperty("retrieval_reference_number")]
        public string RetrievalReferenceNumber { get; set; }
        [JsonProperty("selected_product_code")]
        public string SelectedProductCode { get; set; }
        [JsonProperty("merchant_transaction_id")]
        public string MerchantTransactionId { get; set; }
        [JsonProperty("transmission_date_time")]
        public string TransmissionDateTime { get; set; }
    }
}
