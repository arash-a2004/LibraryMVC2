using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Application.Interfaces
{
    public interface ILibrarianService
    {
        Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input);
        Task CreateNewBook(CreateBookDto input);
        Task DeleteBook(int bookId);
        Task<List<PendingRequestDto>> GetPendingRequests();
        Task ChangePendingStatus(int id, string newStatus);


    }

}
