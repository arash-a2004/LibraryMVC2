using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;

namespace LibrarySystem.Application.Services.LibrarianServices
{
    public class LibarianService : ILibrarianService
    {
        private readonly ILibrarianRepository _librarianRepository;
        public LibarianService(ILibrarianRepository librarianRepository)
        {
            _librarianRepository = librarianRepository;
        }

        public async Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input)
        {
            return await _librarianRepository.GetAllBook(input);
        }

        public async Task CreateNewBook(CreateBookDto input)
        {
            await _librarianRepository.CreateNewBook(input);
        }

        public async Task DeleteBook(int bookId)
        {
            await _librarianRepository.DeleteBook(bookId);
        }

        public async Task<List<PendingRequestDto>> GetPendingRequests()
        {
            return await _librarianRepository.GetPendingRequests();
        }

        public async Task ChangePendingStatus(int id, string newStatus)
        {
            await _librarianRepository.ChangePendingStatus(id, newStatus);
        }

        public async Task<List<GetAllUsersOutput>> GetAllUser(GetAllUsersInput input)
        {
            return await _librarianRepository.GetAllUser(input);
        }

        public async Task<UserDetailOutput> GetUserDetailById(int Id)
        {
            return await _librarianRepository.GetUserDetailById(Id);
        }

    }
}
