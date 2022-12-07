using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;


namespace Nop.Plugin.Widgets.BSEmployee
{
    public class BSEmployeePlugin : BasePlugin, IWidgetPlugin,IAdminMenuPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IScheduleTaskService _scheduleTaskService;

        public BSEmployeePlugin(IWebHelper webHelper,ILocalizationService localizationService,
                                IScheduleTaskService scheduleTaskService)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
        }
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsBSEmployee";
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageTop});
        }
        //Menu te kichu add korte chaile 
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var bSEmployee = new SiteMapNode()
            {
                Title = "BS Employee",
                Url = $"{_webHelper.GetStoreLocation()}Admin/BSEmployee/List",// Folder,controller,Action name
                Visible = true,
                IconClass = "far fa-dot-circle",
                SystemName = "BSEmployee"
            };
            /*var bSEmployee1 = new SiteMapNode()
            {
                Title = "BS Employee",
                Url = $"{_webHelper.GetStoreLocation()}Admin/BSEmployee/List",
                Visible = true,
                IconClass = "far fa-dot-circle",
                SystemName = "BSEmployee"
            };
            bSEmployee.ChildNodes.Add(bSEmployee1);*/
            rootNode.ChildNodes.Add(bSEmployee);
            //bottom up process 
        }

        public override async Task InstallAsync()
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.Employee.EmployeeName"] = "Name",
                ["Plugins.Widgets.Employee.EmployeeName.Hint"] = "The Employee Name",
                ["Plugins.Widgets.Employee.EmployeeBSId"] = "BS Id",
                ["Plugins.Widgets.Employee.EmployeeBSId.Hint"] = "BS Employeee BSId",
                ["Plugins.Widgets.Employee.EmployeeDesingation"] = "Designation",
                ["Plugins.Widgets.Employee.EmployeeDesingation.Hint"] = "The BS Employee Designation",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.PageTitle"] = "BS Employee",
                ["Plugins.Widgets.Employee.Admin.EmployeeList"] = "BS Employee",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeName"] = "Name",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeDesignation"] = "Designation",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeBsId"] = "BS-Id",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.PageTitle.AddNew"] = "Add a new employee",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.BackToList"] = "Back to employee list",
                ["Plugins.Widgets.Employee.Admin.EditEmployeeDetails"] = "Edit employee details",
                ["Plugins.Widgets.Employee.Admin.Employees.Info"] = "Employee Details",
                ["Plugins.Widgets.Employee.EmployeeSearchModel.EmployeeName"] = "Employee Name",
                ["Plugins.Widgets.Employee.EmployeeSearchModel.EmployeeName.Hint"] = "Seachr by a Employee Name",
                ["Plugins.Widgets.Employee.EmployeeSearchModel.EmployeeDesignation"] = "Employee Designation",
                ["Plugins.Widgets.Employee.EmployeeSearchModel.EmployeeDesignation.Hint"] = "Search By a Employee Designation",
            });

            var bsEmployeeScheduleTask = new ScheduleTask()
            {
                Type = "Nop.Plugin.Widgets.BSEmployee.Tasks.BSEmployeeScheduleTask",//BSEmployeeScheduleTask class er (namespace dot class name)
                //namespace = "Nop.Plugin.Widgets.BSEmployee.Tasks" 
                Name = "Send infromtation",
                Seconds = 90,
                Enabled = true,
                StopOnError = false,

            };
           await _scheduleTaskService.InsertTaskAsync(bsEmployeeScheduleTask);
            var bsEmployeeEmailScheduleTask = new ScheduleTask()
            {
                Type = "Nop.Plugin.Widgets.BSEmployee.Tasks.BSEmployeeEmailScheduleTask",
                Name = "Send Email",
                Seconds = 80,
                Enabled = true,
                StopOnError = false,
            };
            await _scheduleTaskService.InsertTaskAsync(bsEmployeeEmailScheduleTask);

            await base.InstallAsync();
        }
    }
}
