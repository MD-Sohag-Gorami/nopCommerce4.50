using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models
{
    public record EmployeeSearchModel:BaseSearchModel
    {
        public EmployeeSearchModel()
        {
            AvailableDesignation = new List<SelectListItem>();
        }
        public string EmployeeName { get; set; }
        public int EmployeeDesignation { get; set; }
        public IList<SelectListItem> AvailableDesignation { get; set; }
    }
}
