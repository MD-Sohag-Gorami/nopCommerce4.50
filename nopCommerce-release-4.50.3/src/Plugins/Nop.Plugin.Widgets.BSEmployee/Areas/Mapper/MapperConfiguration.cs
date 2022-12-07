using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Infrastructure.Mapper;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Mapper
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        public int Order => 1;
        public MapperConfiguration()
        {
            CreateMap<Employee, EmployeeModel>()
            .ForMember(model => model.AvailableDesignation, options => options.Ignore())
            .ForMember(model => model.Designation, options => options.Ignore());
            CreateMap<EmployeeModel, Employee>();
               
        }
    }
}
