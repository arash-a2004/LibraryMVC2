using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.ModelDto;
using System;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Infra
{
    public class AccountingRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountingRepository(AppDbContext dbContext)
        {
           _dbContext = dbContext;
        }
        //create user process
        public async Task Sign_Up(UserSignUpDto input, Role role, string Hash)
        {
            var user = new User()
            {
                Username = input.Username,
                PasswordHash = Hash,
                Role = role,
                SubscriptionTime = DateTime.Now.AddDays(30)
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
            
        }

    }
}
