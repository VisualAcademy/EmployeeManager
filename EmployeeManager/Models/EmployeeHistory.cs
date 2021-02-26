using System;

namespace EmployeeManager.Models
{
    public class EmployeeHistory
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DOB { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Gender { get; set; }

        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Remarks { get; set; }
    }
}
