using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.FlexiCards.Components
{
    [ViewComponent(Name = "FlexiCardsPayment")]
    public class FlexiCardsPaymentViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payment.FlexiCards/Views/PaymentInfo.cshtml");
            
        }
    }
    
}
