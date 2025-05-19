using System;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int? LoanTransactionId { get; set; } // Foreign Key

        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual LoanTransaction LoanTransaction { get; set; }
    }

}
