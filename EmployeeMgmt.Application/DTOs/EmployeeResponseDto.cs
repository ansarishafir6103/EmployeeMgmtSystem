using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
