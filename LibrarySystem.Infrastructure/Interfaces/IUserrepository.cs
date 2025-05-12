using LibrarySystem.Domain.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IUserrepository
    {
        Task<User> GetUserById(int userId);
        Task<List<User>> GetListOfUsers();
    }
}
