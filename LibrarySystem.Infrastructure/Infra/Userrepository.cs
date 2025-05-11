using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Infra
{
    public class Userrepository
    {
        private readonly AppDbContext _dbContext;

        public Userrepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserById(int userId)
        {
            var user = await _dbContext.Users
                .Where(e => e.Id == userId)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                throw new ExceptionHandler.NotFoundException("", "there is no user with this Id");
            }
            return user;
        }


    }
}
