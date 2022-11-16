using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HelloWorld.Components
{
    [ViewComponent(Name = "WidgetsNewHelloWorld")]
    public class WidgetsNewHelloWorldViewComponent : NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone)
        {
           
                return View("~/Plugins/Widgets.HelloWorld/Views/CreateTable.cshtml", widgetZone);
           
        }

    }
}
