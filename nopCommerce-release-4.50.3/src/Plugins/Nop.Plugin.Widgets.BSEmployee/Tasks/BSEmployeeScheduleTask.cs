using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Logging;
using Nop.Services.Logging;
using Nop.Services.ScheduleTasks;

namespace Nop.Plugin.Widgets.BSEmployee.Tasks
{
    public class BSEmployeeScheduleTask : IScheduleTask
    {
        private readonly ILogger _logger;
        public BSEmployeeScheduleTask(ILogger logger)
        {
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            //message show 
            await _logger.InsertLogAsync(LogLevel.Information, "Log from BSEployee Schedule Task");
            
        }
    }
}
