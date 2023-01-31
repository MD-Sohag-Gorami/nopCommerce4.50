using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Payments.FlexiCards.Models
{
    public class PaymentUrlRequest
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }
        [JsonProperty("login_id")]
        public string LoginId { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("merchant_transaction_id")]
        public string MerchantTransactionId { get; set; }
        [JsonProperty("transaction_amount")]
        public int TransactionAmount { get; set; }
        [JsonProperty("include_product_codes")]
        public ProductCodes IncludeProductCodes { get; set; }
        [JsonProperty("exclude_product_codes")]
        public ProductCodes ExcludeProductCodes { get; set; }
        [JsonProperty("url_response")]
        public string UrlResponse { get; set; }
        [JsonProperty("direct_to_url_response")]
        public bool DirectToUrlResponse { get; set; }
        [JsonProperty("lineItems")]
        public LineItems LineItems { get; set; }
        [JsonProperty("transmission_date_time")]
        public string TransmissionDateTime { get; set; }
    }
}
