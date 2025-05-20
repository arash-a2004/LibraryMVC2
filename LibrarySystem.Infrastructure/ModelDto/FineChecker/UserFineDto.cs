using System;

namespace LibrarySystem.Infrastructure.ModelDto.FineChecker
{
    public class UserFineCombinedDto
    {
        public int UserId { get; set; }
        public int LoanTransactionId { get; set; }
        public string BookTitle { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? DaysLate { get; set; }
        public decimal? FineAmount { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsReturned => ReturnDate != null;
    }


}
