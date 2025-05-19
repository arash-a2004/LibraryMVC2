using System;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class Fine
    {
        public int Id { get; set; }

        public int LoanTransactionId { get; set; } 

        public int? DaysLate { get; set; } 
        public decimal? FineAmount { get; set; } 

        public bool IsPaid { get; set; } 
        public DateTime? PaymentDate { get; set; } 

        // Navigation property
        public virtual LoanTransaction LoanTransaction { get; set; }
    }


}
