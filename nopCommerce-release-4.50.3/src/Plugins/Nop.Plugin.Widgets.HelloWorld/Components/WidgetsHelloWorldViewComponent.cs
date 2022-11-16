using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HelloWorld.Components
{
    [ViewComponent(Name = "WidgetsHelloWorld")]
    public class WidgetsHelloWorldViewComponent: NopViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone)
        {
            var name = "Hello World";

            return View("~/Plugins/Widgets.HelloWorld/Views/Default.cshtml", name);
        }

    }
}
