using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.BSEmployee
{
    public class BSEmployeePlugin : BasePlugin, IWidgetPlugin,IAdminMenuPlugin
    {
        private readonly IWebHelper _webHelper;

        public BSEmployeePlugin(IWebHelper webHelper)
        {
            _webHelper = webHelper;
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
            rootNode.ChildNodes.Add(bSEmployee);
        }
    }
}
