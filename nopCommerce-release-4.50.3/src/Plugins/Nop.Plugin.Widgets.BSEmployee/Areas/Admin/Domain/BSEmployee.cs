using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain
{
    public partial class BSEmployee : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeBsId { get; set; }
        public int   Designation { get; set; }

    }
}
