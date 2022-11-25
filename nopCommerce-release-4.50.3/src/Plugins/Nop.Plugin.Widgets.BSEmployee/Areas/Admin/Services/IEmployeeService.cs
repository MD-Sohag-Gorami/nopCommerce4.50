using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services
{
    public interface IEmployeeService
    {
        Task DeleteEmployeeByIdAsync(Employee employee);
        Task<IPagedList<Employee>> GetAllEmployeesAsync(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task InsertEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);

        Task<IPagedList<Employee>> SearchEmployeesAsync(bool showHidden = false, int designationId = 0,
            string employeeName = "", int pageIndex = 0, int pageSize = int.MaxValue

           );
    }
}