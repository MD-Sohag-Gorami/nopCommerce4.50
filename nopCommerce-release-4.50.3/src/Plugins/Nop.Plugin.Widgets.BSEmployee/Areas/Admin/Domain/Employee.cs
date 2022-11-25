using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain
{
    public partial class Employee : BaseEntity
    {
       
        public string EmployeeName { get; set; }
        public string EmployeeBsId { get; set; }
        public int DesignationId { get; set; }
        public Designation Designation
        {
            get => (Designation)DesignationId;
            set => DesignationId = (int)value;
        }

    }
}
