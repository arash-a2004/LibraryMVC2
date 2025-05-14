using System;

namespace LibrarySystem.Domain.Models.DbModels
{
    public class Fine
    {
        public int Id { get; set; }

        public int LoanTransactionId { get; set; } // Foreign Key به LoanTransaction

        public int DaysLate { get; set; } 
        public int FineAmount { get; set; } 
        public DateTime FineDate { get; set; } 

        public bool IsPaid { get; set; } 
        public DateTime? PaymentDate { get; set; } 

        // Navigation property
        public virtual LoanTransaction LoanTransaction { get; set; }
    }


}
