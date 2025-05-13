using System;

namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public sealed class GetAllBooksInput
    {
        public string Title { get; set; } = string.Empty ;
        public string Author { get; set; } = string.Empty;
        public bool? IsAvailable { get; set; } = true;
        public int SkipCount { get; set; } = 0;
        public int MaxResult { get; set; } = 10;


    }


}
