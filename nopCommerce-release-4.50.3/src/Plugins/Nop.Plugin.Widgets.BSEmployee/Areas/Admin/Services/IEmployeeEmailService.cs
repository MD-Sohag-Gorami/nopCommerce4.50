using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services
{
    public interface IEmployeeEmailService
    {
        Task<IList<int>> SendBSEmployeeCreateNotificationAsync(Employee employee, int languageId);
    }
}