using System;
using System.Collections.Generic;

namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public class UserDetailOutput
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime? SubscriptionTime { get; set; }
        public bool IsActive { get; set; } = true;
        public List<LoanRequests> LoanRequests { get; set; }
    }

    public class LoanRequests
    {
        public string BookTitle { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected

    }
}
