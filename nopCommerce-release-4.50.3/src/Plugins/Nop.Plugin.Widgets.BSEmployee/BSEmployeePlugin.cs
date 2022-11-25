using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.BSEmployee
{
    public class BSEmployeePlugin : BasePlugin, IWidgetPlugin,IAdminMenuPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public BSEmployeePlugin(IWebHelper webHelper,ILocalizationService localizationService)
        {
            _webHelper = webHelper;
            _localizationService = localizationService;
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

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var bSEmployee = new SiteMapNode()
            {
                Title = "BS Employee",
                Url = $"{_webHelper.GetStoreLocation()}Admin/BSEmployee/List",
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
                ["Plugins.Widgets.Employee.EmployeeName"] = "Enter Employee Name",
                ["Plugins.Widgets.Employee.EmployeeBSId"] = "Enter Employee BS Id",
                ["Plugins.Widgets.Employee.EmployeeDesingation"] = "Select Employee Designation",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.PageTitle"] = "BS Employee",
                ["Plugins.Widgets.Employee.Admin.EmployeeList"] = "BS Employee",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeName"] = "Name",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeDesignation"] = "Designation",
                ["Plugins.Widgets.Employee.Admin.Fields.EmployeeBsId"] = "BS-Id",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.PageTitle.AddNew"] = "Add a new employee",
                ["Plugins.Widgets.Employee.Admin.EmployeeList.BackToList"] = "Back to employee list",
                ["Plugins.Widgets.Employee.Admin.EditEmployeeDetails"] = "Edit employee details",
                ["Plugins.Widgets.Employee.Admin.Employees.Info"] = "Employee Details",
               


            });

            await base.InstallAsync();
        }
    }
}
