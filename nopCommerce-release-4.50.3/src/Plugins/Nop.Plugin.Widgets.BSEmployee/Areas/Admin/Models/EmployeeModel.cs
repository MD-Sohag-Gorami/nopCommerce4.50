using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models
{
    public record EmployeeModel : BaseNopEntityModel
    {
        public EmployeeModel()
        {
            AvailableDesignation = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Widgets.Employee.EmployeeName")]
        public string EmployeeName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Employee.EmployeeBSId")]
        public string EmployeeBsId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Employee.EmployeeDesingation")]
        public int DesignationId { get; set; }
        public IList<SelectListItem> AvailableDesignation { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Employee.EmployeeDesingation")]
        public string  Designation { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.Employee.EmployeeAge")]
        public int EmployeeAge { get; set; } 


    }
}
