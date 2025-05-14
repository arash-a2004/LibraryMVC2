using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto
{
    public sealed class CreateBookDto
    {

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

    }

}
