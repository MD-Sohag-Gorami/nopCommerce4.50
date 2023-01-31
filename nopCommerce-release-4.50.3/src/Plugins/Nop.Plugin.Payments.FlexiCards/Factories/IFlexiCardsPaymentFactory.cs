using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.FlexiCards.Models;

namespace Nop.Plugin.Payments.FlexiCards.Factories
{
    public interface IFlexiCardsPaymentFactory
    {
        string PrepareFlexiCardsPaymentStatusQueryString(string requestUrl, PaymentStatusRequest requestBody);
        List<string> PrepareFlexiCardsProducts();
        Task<PaymentUrlRequest> PreparePaymentUrlRequest(FlexiCardsPaymentSettings settings, Order order);
    }
}