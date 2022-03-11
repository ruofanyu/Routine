using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Routine.API.Entities;
using Routine.API.ValidationAttributes;

namespace Routine.API.Models
{
    [EmployeeNoMustDifferentFromFirstName]
    public class EmployeeAddDto : EmployeeAddOrUpdateDto
    {
    }
}
