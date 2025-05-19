using System;

namespace LibrarySystem.Infrastructure.ModelDto.MemberDto
{
    public class LoanRequestListDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected, BackgroundCheck

        public string BookTitle { get; set; }

    }
}
