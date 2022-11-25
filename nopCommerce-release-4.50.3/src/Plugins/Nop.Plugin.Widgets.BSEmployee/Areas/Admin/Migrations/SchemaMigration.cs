using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Migrations
{
    [NopMigration("2022/11/16 08:49:55:1687541", "Widgets.BSEmployee base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Employee>();
        }
    }
}
