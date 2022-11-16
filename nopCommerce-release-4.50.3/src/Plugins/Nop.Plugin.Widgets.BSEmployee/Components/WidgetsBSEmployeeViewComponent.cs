using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BSEmployee.Components
{
    [ViewComponent(Name = "WidgetsBSEmployee")]
    public class WidgetsBSEmployeeViewComponent: NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone)
        {
            var name = "BS Employee";

            return View("~/Plugins/Widgets.HelloWorld/Views/Default.cshtml", name);
        }
    }
}
