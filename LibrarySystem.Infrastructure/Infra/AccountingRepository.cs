using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.BCryptServices;
using LibrarySystem.Infrastructure.ExceptionHandler;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AccountingDto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnauthorizedAccessException = LibrarySystem.Infrastructure.ExceptionHandler.UnauthorizedAccessException;

namespace LibrarySystem.Infrastructure.Infra
{
    public class AccountingRepository : IAccountingRepository
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

            if (_dbContext.Users.Any(e => e.Username == user.Username))
            {
                throw new InUseException("Username is already in use.", "Username is already in use.");
            }

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

        }

        //sign-in functionality
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

        //delete user 
        public async Task deleteUser(int UserId)
        {
            var user = await _dbContext.Users.FindAsync(UserId);

            if (user is null)
                throw new ExceptionHandler.NotFoundException("", "there is no user with this Id");

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        //Edit -> password
        public async Task ChangeUserPassword(int userId, string newPassword)
        {
            //create a change password functionality
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
                throw new ExceptionHandler.NotFoundException("", "there is no user with this Id");

            user.PasswordHash = new PasswordHashingServices().HashPassword(newPassword);
            _dbContext.Entry(user).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }


        //Edit -> Role 
        public async Task ChangeUserRole(int userId, Role role = Role.Member)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user is null)
                throw new ExceptionHandler.NotFoundException("", "there is no user with this Id");
            user.Role = role;
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


    }
}
