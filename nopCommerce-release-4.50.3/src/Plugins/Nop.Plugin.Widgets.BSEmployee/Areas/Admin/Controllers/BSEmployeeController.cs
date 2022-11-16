using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Controllers
{
    public class BSEmployeeController: BaseAdminController
    {
        public async Task<IActionResult> List()
        {
            var list = new List<string>();
            list.Add("Sohag");
            list.Add("Faizul");
            list.Add("Atiqur vai");

            return View("~/Plugins/Widgets.BSEmployee/Views/BSEmployee/List.cshtml", list);
        }
    }
}
