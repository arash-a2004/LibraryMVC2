using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;

namespace LibrarySystem.Application.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<GetAllUsersOutput>> GetAllUser(GetAllUsersInput input)
        {
            return await _adminRepository.GetAllUser(input);
        }

        public async Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input)
        {
            return await _adminRepository.GetAllBook(input);
        }

        public async Task<UserDetailOutput> GetUserDetailById(int Id)
        {
            return await _adminRepository.GetUserDetailById(Id);
        }
    }
}
