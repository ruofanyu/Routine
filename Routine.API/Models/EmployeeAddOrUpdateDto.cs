using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Routine.API.Entities;
using Routine.API.ValidationAttributes;

namespace Routine.API.Models
{

  [EmployeeNoMustDifferentFromFirstName]
    public class EmployeeAddOrUpdateDto : IValidatableObject
    {
        [Display(Name = "员工号")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0}的长度是{1}")]
        public string EmployeeNo { get; set; }

        [Display(Name = "名")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(50, ErrorMessage = "{0}的长度不能超过{1}")]
        public string FirstName { get; set; }

        [Display(Name = "姓"), Required(ErrorMessage = "{0}是必填的"), MaxLength(50, ErrorMessage = "{0}的长度是{1}")]
        public string LastName { get; set; }

        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        [Display(Name = "出生日期")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("姓和名不能一致",
                    new[] { nameof(EmployeeAddDto) });      //因为是跨属性的错误，所以我们给他定义一个类的错误
                                                            //new[] { nameof(LastName), nameof(FirstName) });         //也可以定义为两个属性的错误
            }
        }
    }

}
