using System;

namespace LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto
{
    public class PendingRequestDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string BookTitle { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }


}
