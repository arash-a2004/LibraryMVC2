using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IAccountingRepository
    {
        Task Sign_Up(UserSignUpDto input);
        Task<User> Sign_In(UserSignInDto input);
        Task deleteUser(int UserId);
        Task ChangeUserPassword(int userId, string newPassword);
        Task ChangeUserRole(int userId, Role role = Role.Member);

    }
}
