using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services;
using Nop.Services;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Factories
{
    public class EmployeeModelFactory : IEmployeeModelFactory
    {
        #region Ctor
        private readonly IEmployeeService _employeeService;
        #endregion
        #region Ctor
        public EmployeeModelFactory(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion
        #region Methods
        public async Task<EmployeeModel> PrepareEmployeeModelAsync(EmployeeModel employeeModel, Employee employee)
        {
            var model = new EmployeeModel();

            if (employee != null)
            {
                model.Id = employee.Id;
                model.EmployeeName = employee.EmployeeName;
                model.EmployeeBsId = employee.EmployeeBsId;
                model.DesignationId = employee.DesignationId;
            }
            var availableDesignation = (await Designation.ProjectManager.ToSelectListAsync(false)).Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value,
            }).ToList();

            availableDesignation.Insert(0, new SelectListItem()
            {
                Text = "All",
                Value = ""
            });
            model.AvailableDesignation = availableDesignation;

            return model;
        }

        public async Task<EmployeeSearchModel> PrepareEmployeeSearchModelAsync(EmployeeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            searchModel.SetGridPageSize();

            var availableDesignation = (await Designation.ProjectManager.ToSelectListAsync(false)).Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value,
            }).ToList();

            availableDesignation.Insert(0, new SelectListItem()
            {
                Text = "All",
                Value = ""
            });
            searchModel.AvailableDesignation = availableDesignation;
            return searchModel;
        }
        public async Task<EmployeeListModel> PrepareEmployeeListModelAsync(EmployeeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter comments
            var overrideEmployeeName = searchModel.EmployeeName;
            if (searchModel.EmployeeName == null)
                overrideEmployeeName = "";

            var designationId = new List<int> { searchModel.EmployeeDesignation };

            //get employees
            var employees = await _employeeService.SearchEmployeesAsync(showHidden: true,
                designationId: searchModel.EmployeeDesignation,
                employeeName: searchModel.EmployeeName,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
                );

            //prepare list model
            var model = await new EmployeeListModel().PrepareToGridAsync(searchModel, employees, () =>
            {
                return employees.SelectAwait(async employee =>
                {
                    //fill in model values from the entity
                    //var employeeModel = employee.ToModel<EmployeeModel>();
                    var employeeModel = new EmployeeModel()
                    {
                        Id = employee.Id,
                        EmployeeName = employee.EmployeeName,
                        EmployeeBsId = employee.EmployeeBsId,
                        Designation = employee.Designation.ToString(),


                    };

                    return employeeModel;
                });
            });

            return model;
        }
        public async Task<EmployeeModel> PrepareEmployeeByIdAsync(int id)
        {
            EmployeeModel employee = new EmployeeModel();
            return employee;
        }
        #endregion
    }
}
