using System;
using System.Collections.Generic;

namespace LibrarySystem.Web.Models
{
    public class UserFineDetailsViewModel
    {
        public int UserId { get; set; }
        public int LoanRequestId { get; set; }

        public string RequestedBook { get; set; }
        public DateTime RequestDate { get; set; }

        public List<UserFineCombinedViewModel> UnreturnedFines { get; set; } = new List<UserFineCombinedViewModel>();
        public List<UserFineCombinedViewModel> ReturnedFines { get; set; } = new List<UserFineCombinedViewModel>();
    }
    public class UserFineCombinedViewModel
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