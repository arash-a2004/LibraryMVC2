using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using System.Threading.Tasks;

namespace LibrarySystem.Application.Interfaces
{
    public interface IAccountingServices
    {
        Task ChangeUserRole(int userId, Role changerRole, Role role = Role.Member);
        Task ChangeUserPassword(int userId, string newPassword);
        Task DeleteUser(int userId);
        Task<User> SignIn(UserSignInDto input);
        Task SignUp(UserSignUpDto input);
    }
}
