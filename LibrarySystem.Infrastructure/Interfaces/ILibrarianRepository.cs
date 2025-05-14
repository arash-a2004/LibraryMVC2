using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface ILibrarianRepository
    {
        Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input);
        Task CreateNewBook(CreateBookDto input);
        Task DeleteBook(int bookId);
        Task<List<PendingRequestDto>> GetPendingRequests();
        Task ChangePendingStatus(int id, string newStatus);
    }
}
