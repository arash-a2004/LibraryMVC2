using System;
using System.Collections.Generic;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } //$2a$11$84PpoHfttCMO2dZKoxzrtu4fkYaYsSshxmo7bRBQJfdbbS1oWTYHS->123456
        public Role Role { get; set; }
        public DateTime SubscriptionTime { get; set; }
        public bool IsActive { get; set; } = true;
        // Navigation properties
        public virtual ICollection<LoanRequest> LoanRequests { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }

    }

}
