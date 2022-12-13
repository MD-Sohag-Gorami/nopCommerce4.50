using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Models;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Validators
{
    public class EmployeeModelValidators : BaseNopValidator<EmployeeModel>
    {
        public EmployeeModelValidators(IEmployeeService employeeService)
        {
            //set validation rules
            RuleFor(model => model.EmployeeAge)
                .GreaterThan(0)
                .WithMessage("Number should be greater than zero");

            RuleFor(model => model.EmployeeName)
                .NotEmpty()
                .WithMessage("Kichu ekta lekhe jan dada !!!");
            RuleFor(model => model.EmployeeBsId)
                .NotEmpty()
                .WithMessage("BS ID na diya koi jan ? !!!");
            RuleFor(x => x.EmployeeBsId)
              .NotEmpty().WithMessage("BS ID na diya koi jan ? !!!")
              .Must((x, context) =>
              {
                  
                  return employeeService.ValidateEmployeeBsIdUniqueAsync(x.EmployeeBsId, x.Id).Result;
              })
              .WithMessage("The given BSID already assigned");


            RuleFor(model => model.DesignationId)
                .GreaterThan(0)
                .WithMessage("Number should be greater than zero");


        }

    }
}
