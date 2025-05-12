using System;
using System.Collections.Generic;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class LoanTransaction
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public int UserId { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int LoanRequestId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }

        // One-to-one
        public virtual LoanRequest LoanRequest { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new HashSet<ActivityLog>();

    }

}
