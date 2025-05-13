using System;
using System.Collections.Generic;

namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public class BookDetailOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<transaction> transactions { get; set; }
    }
    public class transaction
    {
        public string Username { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }


}
