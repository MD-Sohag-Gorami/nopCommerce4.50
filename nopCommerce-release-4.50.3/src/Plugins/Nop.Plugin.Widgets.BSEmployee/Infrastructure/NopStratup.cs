using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Factories;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Infrastructure
{
    public class NopStratup : INopStartup
    {
        public int Order => int.MaxValue;

        public void Configure(IApplicationBuilder application)
        {
           
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeModelFactory, EmployeeModelFactory>();
            services.AddScoped<IEmployeeEmailService, EmployeeEmailService>();
            

        }


    }
}
