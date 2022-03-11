using System.ComponentModel.DataAnnotations;
using Routine.API.Models;

namespace Routine.API.ValidationAttributes
{
    public class EmployeeNoMustDifferentFromFirstNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //其中object value跟据EmployeeNoMustDifferentFromFirstNameAttribute所使用的地方不一样，如果在属性上使用，
            //则value是属性级别，如果在类上使用，则value是类级别，故我们一般使用value来进行判断
            var addDto = (EmployeeAddOrUpdateDto)validationContext.ObjectInstance;
            if (addDto.EmployeeNo == addDto.FirstName)
            {
                return new ValidationResult("员工编号不可以等于名", new[] { nameof(EmployeeAddOrUpdateDto) });
            }

            return ValidationResult.Success;
        }
    }
}
