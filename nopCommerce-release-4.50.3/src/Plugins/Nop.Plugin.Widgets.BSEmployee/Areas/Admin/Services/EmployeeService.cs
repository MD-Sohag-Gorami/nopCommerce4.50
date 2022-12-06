using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services
{
    public class EmployeeService : IEmployeeService
    {
        #region Fileds
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly IWorkContext _workContext;
        private readonly IEmployeeEmailService _employeeEmailService;
        #endregion
        #region Ctor
        public EmployeeService(IRepository<Employee> employeeRepository,
                               IRepository<LocalizedProperty> localizedPropertyRepository,
                               IWorkContext workContext,
                               IEmployeeEmailService employeeEmailService
                              )
        {
            _employeeRepository = employeeRepository;
            _localizedPropertyRepository = localizedPropertyRepository;
            _workContext = workContext;
            _employeeEmailService = employeeEmailService;
        }
        #endregion
        #region Methods
        public async Task<IPagedList<Employee>> GetAllEmployeesAsync(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _employeeRepository.Table;
            return await query.ToPagedListAsync<Employee>(pageIndex, pageSize);
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var query = _employeeRepository.Table;


            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }
        public async Task InsertEmployeeAsync(Employee employee)
        {
            await _employeeRepository.InsertAsync(employee);
            var employeeId = employee.Id;
            var languageId = ( await _workContext.GetWorkingLanguageAsync()).Id ;
            await _employeeEmailService.SendBSEmployeeCreateNotificationAsync(employee, languageId);


        }

        public async Task DeleteEmployeeByIdAsync(Employee employee)
        {
            await _employeeRepository.DeleteAsync(employee);
        }

        public async Task<IPagedList<Employee>> SearchEmployeesAsync(bool showHidden = false, int designationId = 0,
            string employeeName = "", int pageIndex = 0, int pageSize = int.MaxValue

           )
        {
           
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            var employeeQuery = _employeeRepository.Table;
            if (!string.IsNullOrEmpty(employeeName))
                employeeQuery = employeeQuery.Where(x => x.EmployeeName == employeeName);
            if (designationId != 0)
                employeeQuery = employeeQuery.Where(x => x.DesignationId == designationId);

            return await employeeQuery.ToPagedListAsync(pageIndex, pageSize);

            // return await employeeQuery.OrderBy(_localizedPropertyRepository, await _workContext.GetWorkingLanguageAsync(), orderBy).ToPagedListAsync(pageIndex, pageSize);
        }
        #endregion

    }
}
