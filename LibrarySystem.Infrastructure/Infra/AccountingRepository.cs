using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.BCryptServices;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnauthorizedAccessException = LibrarySystem.Infrastructure.ExceptionHandler.UnauthorizedAccessException;

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
        public async Task Sign_Up(UserSignUpDto input)
        {
            var user = new User()
            {
                Username = input.Username,
                PasswordHash = new PasswordHashingServices().HashPassword(input.Password),
                Role = Role.Member,
                SubscriptionTime = DateTime.Now.AddDays(30)
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

        }


        public async Task<User> Sign_In(UserSignInDto input)
        {
            var user = await _dbContext.Users
                .Where(e => e.Username == input.Username)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new UnauthorizedAccessException(null, "username should be filled first ");
            }

            if (!new PasswordHashingServices().VerifyPassword(input.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException();
            }

            return user;
        }
    }
}
