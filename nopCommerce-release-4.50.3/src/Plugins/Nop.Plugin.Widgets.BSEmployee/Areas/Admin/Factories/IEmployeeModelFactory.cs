using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Factories
{
    public interface IEmployeeModelFactory
    {
      
        Task<EmployeeModel> PrepareEmployeeByIdAsync(int id);
        Task<EmployeeModel> PrepareEmployeeModelAsync(EmployeeModel employeeModel,Employee employee);
        Task<EmployeeSearchModel> PrepareEmployeeSearchModelAsync(EmployeeSearchModel searchModel);

        Task<EmployeeListModel> PrepareEmployeeListModelAsync(EmployeeSearchModel searchModel);
    }
}