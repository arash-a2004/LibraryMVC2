using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;

namespace LibrarySystem.Application.Services.UserServices
{
    public class UserServices
    {
        private readonly IUserrepository _userrepository;

        public UserServices(IUserrepository userrepository)
        {
            _userrepository = userrepository;
        }

        public async Task<User> GetUserById(Role role, int Id)
        {
            if(role != Role.Member) 
                return null;

            var res = await _userrepository.GetUserById(Id);

            return res;
        }

        public async Task<List<User>> GetAllUsers(Role role)
        {
            if (role != Role.Member)
                return null;

            return await _userrepository.GetListOfUsers();
        }
    }
}
