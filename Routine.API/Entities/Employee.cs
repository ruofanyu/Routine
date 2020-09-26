using System;

namespace Routine.API.Entities
{
    public class Employee
    {
        public Guid  Id { get; set; }

        public Guid CompanyId { get; set; }

        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfbirth{ get; set; }
        public Company Company{ get; set; }
    }
}