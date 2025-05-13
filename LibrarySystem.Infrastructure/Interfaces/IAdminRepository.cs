using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<GetAllUsersOutput>> GetAllUser(GetAllUsersInput input);
        Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input);
        Task<UserDetailOutput> GetUserDetailById(int Id);
        Task<BookDetailOutput> GetBookDetails(int Id);
    }
}
