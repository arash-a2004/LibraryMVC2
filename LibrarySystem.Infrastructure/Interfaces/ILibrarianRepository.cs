using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface ILibrarianRepository
    {
        Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input);
        Task CreateNewBook(CreateBookDto input);
        Task DeleteBook(int bookId);
    }
}
