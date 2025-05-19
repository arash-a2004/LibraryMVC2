using System;

namespace LibrarySystem.Web.Models
{
    public class LoanRequestListViewModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected, BackgroundCheck

        public string BookTitle { get; set; }
    }
}