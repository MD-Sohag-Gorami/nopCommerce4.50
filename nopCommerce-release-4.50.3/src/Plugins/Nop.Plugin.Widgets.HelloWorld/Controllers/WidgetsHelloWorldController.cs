using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.HelloWorld.Controllers
{
    public class WidgetsHelloWorldController : BasePluginController
    {
        public async Task<IActionResult> Index()
        {
            var componentName = "Widgets.HelloWorld";
            return View("~/Plugins/Widgets.HelloWorld/Views/Index.cshtml", componentName);
        }
    }
}
