using System;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class LoanRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int BookId { get; set; }

        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }

        // One-to-one
        public virtual LoanTransaction LoanTransaction { get; set; }

    }

}
