using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Factories;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Controllers
{
    public class BSEmployeeController: BaseAdminController
    {
        #region Fields
        private readonly IEmployeeModelFactory _employeeModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IEmployeeService _employeeService;
        #endregion
        #region Ctor
        public BSEmployeeController(IEmployeeModelFactory employeeModelFactory,
                                    IPermissionService permissionService,
                                    IEmployeeService employeeService)
        {
            _employeeModelFactory = employeeModelFactory;
            _permissionService = permissionService;
            _employeeService = employeeService;
        }
        #endregion
        #region List
        public  async Task<IActionResult> List()
        {

            //prepare model
            var model = await _employeeModelFactory.PrepareEmployeeSearchModelAsync(new EmployeeSearchModel());

            return View("~/Plugins/Widgets.BSEmployee/Areas/Admin/Views/BSEmployee/List.cshtml", model);
        }

        [HttpPost]
        public  async Task<IActionResult> EmployeeList(EmployeeSearchModel searchModel)
        {
            
            //prepare model
            var model = await _employeeModelFactory.PrepareEmployeeListModelAsync(searchModel);

            return Json(model);
        }
        #endregion
        #region Create
        public  async Task<IActionResult> Create()
        {
            //prepare model
            var model = await _employeeModelFactory.PrepareEmployeeModelAsync(new EmployeeModel(), null); 
            ;

            return View("~/Plugins/Widgets.BSEmployee/Areas/Admin/Views/BSEmployee/Create.cshtml", model);
            
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public  async Task<IActionResult> Create(EmployeeModel model, bool continueEditing)
        {


            if (ModelState.IsValid)
            {
                //var employee = model.ToEntity<Employee>();

                var employee = new Employee()
                {
                    EmployeeBsId = model.EmployeeBsId,
                    EmployeeName = model.EmployeeName,
                    DesignationId = model.DesignationId,
                };

                await _employeeService.InsertEmployeeAsync(employee);

            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            
            if(employee == null) return RedirectToAction("List");
            await _employeeService.DeleteEmployeeByIdAsync(employee);
            return RedirectToAction("List");
        }

        [HttpPost]
        public  async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
        {
         

            if (selectedIds == null || selectedIds.Count == 0)
                return NoContent();
            selectedIds.ToArray();
            foreach(var id in selectedIds)
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return RedirectToAction("List");
                await _employeeService.DeleteEmployeeByIdAsync(employee);
            }

            return Json(new { Result = true });
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            

            //try to get a category with the specified id
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _employeeModelFactory.PrepareEmployeeModelAsync(null, employee);

            return View("~/Plugins/Widgets.BSEmployee/Areas/Admin/Views/BSEmployee/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(EmployeeModel model, bool continueEditing)
        {


            //try to get a employee with the specified id
            var employeeObj = await _employeeService.GetEmployeeByIdAsync(model.Id);
            if (employeeObj == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {


                var employee = new Employee()
                {
                    Id = model.Id,
                    EmployeeBsId = model.EmployeeBsId,
                    EmployeeName = model.EmployeeName,
                    DesignationId = model.DesignationId,
                };
              
                await _employeeService.UpdateEmployeeAsync(employee);

              
                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = employee.Id });
            }

            //prepare model
            model = await _employeeModelFactory.PrepareEmployeeModelAsync(model, employeeObj);
            return View("~/Plugins/Widgets.BSEmployee/Areas/Admin/Views/BSEmployee/Edit.cshtml", model);
        }

        #endregion
    }
}
