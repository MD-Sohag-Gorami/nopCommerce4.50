using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.FlexiCards
{
    public class FlexiCardsPaymentDeafults
    {
        public static string PaymentUrlTestEndPoint => "https://api.flexilongtermfinance.co.nz/api/simulator/gateway/rest/v1/payment/paymenturl";
        public static string PaymentStatusTestEndPoint => "https://api.flexilongtermfinance.co.nz/api/simulator/gateway/rest/v1/payment/paymentstatus";

        public static string PaymentUrlEndPoint => "https://api.flexilongtermfinance.co.nz/api/gateway/rest/v1/payment/paymenturl";
        public static string PaymentStatusEndPoint => "https://api.flexilongtermfinance.co.nz/api/gateway/rest/v1/payment/paymentstatus";

        public static string ResponseSuccessful => "00";
        public static string PaymentStatusApproved => "00";
        public static string PaymentStatusDeclined => "01";
        public static string PaymentStatusInProgress => "02";
        public static string PaymentStatusError => "03";
    }
}
