using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.QueryableExtensions;

namespace LibrarySystem.Infrastructure.Infra
{

    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _appDbContext;

        public AdminRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<GetAllUsersOutput>> GetAllUser(GetAllUsersInput input)
        {
            var usersQuery = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(input.Username))
            {
                usersQuery = usersQuery.Where(u => u.Username.Contains(input.Username));
            }

            if (input.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == input.IsActive.Value);
            }

            var users = await usersQuery
                .Select(u => new GetAllUsersOutput
                {
                    Id = u.Id,
                    Username = u.Username,
                    SubscriptionTime = u.SubscriptionTime,
                    IsActive = u.IsActive
                })
                .OrderBy(u => u.Id)
                .Skip(input.SkipCount)
                .Take(input.MaxResult)
                .ToListAsync();

            return users;

        }

        public async Task<List<GetAllBooksOutput>> GetAllBook(GetAllBooksInput input)
        {
            var BooksQuery = _appDbContext.Books.AsQueryable();

            if (!string.IsNullOrEmpty(input.Title))
            {
                BooksQuery = BooksQuery.Where(u => u.Title.Contains(input.Title));
            }
            if (!string.IsNullOrEmpty(input.Author))
            {
                BooksQuery = BooksQuery.Where(u => u.Author.Contains(input.Author));
            }
            if (input.IsAvailable.HasValue)
            {
                BooksQuery = BooksQuery.Where(u => u.IsAvailable);
            }

            var books = await BooksQuery
                .Select(u => new GetAllBooksOutput()
                {
                    Id = u.Id,
                    Title = u.Title,
                    Author = u.Author,
                    IsAvailable = u.IsAvailable
                })
                 .OrderBy(u => u.Id)
                .Skip(input.SkipCount)
                .Take(input.MaxResult)
                .ToListAsync();

            return books;

        }

        public async Task<UserDetailOutput> GetUserDetailById(int Id)
        {
            return await _appDbContext.Users
                .Include("LoanRequests.Book") // String path for nested includes
                .Where(e => e.Id == Id)
                .Select(e => new UserDetailOutput()
                {
                    Id = e.Id,
                    Username = e.Username,
                    SubscriptionTime = e.SubscriptionTime,
                    IsActive = e.IsActive,
                    LoanRequests = e.LoanRequests.Select(lr => new LoanRequests()
                    {
                        BookTitle = lr.Book.Title,
                        RequestDate = lr.RequestDate,
                        Status = lr.Status
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<BookDetailOutput> GetBookDetails(int Id)
        {
            return await _appDbContext.Books
                .Include("LoanTransaction.User") // String path for nested includes
                .Where(e => e.Id == Id)
                .Select(e => new BookDetailOutput()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Author = e.Author,
                    IsAvailable = e.IsAvailable,
                    transactions = e.LoanTransactions.Select(lr => new transaction()
                    {
                        Username = lr.User.Username,
                        LoanDate = lr.LoanDate,
                        ReturnDate = lr.ReturnDate,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }




    }
}
