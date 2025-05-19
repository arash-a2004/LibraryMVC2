using System;

namespace LibrarySystem.Infrastructure.ModelDto.FineChecker
{
    public class LoanRequestListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string BookTitle { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }

}
