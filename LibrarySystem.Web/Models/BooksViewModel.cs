using System.Collections.Generic;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;

namespace LibrarySystem.Web.Models
{
    public class BooksViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<GetAllBooksOutput> Books { get; set; } = new List<GetAllBooksOutput>();
        public int SkipCount { get; set; } = 0;
        public int MaxResult { get; set; } = 10;

    }
}