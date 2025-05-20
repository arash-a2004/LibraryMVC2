using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.ExceptionHandler;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;

namespace LibrarySystem.Infrastructure.Infra
{
    public class LibrarianRepository : ILibrarianRepository
    {
        private readonly AppDbContext _appDbContext;

        public LibrarianRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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

        public async Task CreateNewBook(CreateBookDto input)
        {
            Book book = new Book();

            book.Title = input.Title;
            book.Author = input.Author;

            _appDbContext.Books.Add(book);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteBook(int bookId)
        {
            var books = await _appDbContext.Books
                .Include(u => u.LoanTransactions)
                .Where(u => u.Id == bookId)
                .FirstOrDefaultAsync();

            if (books is null)
                throw new KeyNotFoundException("there is no Book with this Id");

            if (books.LoanTransactions.Any())
                throw new System.Exception("can not delete this book becaues there are some loantransaction on book");

            _appDbContext.Books.Remove(books);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<PendingRequestDto>> GetPendingRequests()
        {
            return await _appDbContext.LoanRequests
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.Status == "Pending")
                .Select(r => new PendingRequestDto
                {
                    Id = r.Id,
                    Username = r.User.Username,
                    BookTitle = r.Book.Title,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                })
                .ToListAsync();
        }

        public async Task ChangePendingStatus(int id, string newStatus)
        {
            try
            {
                var req = await _appDbContext.LoanRequests
                    .Include(r => r.Book)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (req == null)
                    throw new NotFoundException("pendingrequest not found", id);

                if (newStatus == "Approved")
                {
                    // 1. mark book unavailable
                    req.Book.IsAvailable = false;

                    // 2. create LoanTransaction
                    var txn = new LoanTransaction
                    {
                        BookId = req.BookId,
                        UserId = req.UserId,
                        LoanDate = DateTime.Now,
                        LoanRequestId = req.Id,
                        LoanRequest = req,
                    };
                    _appDbContext.LoanTransactions.Add(txn);

                    await _appDbContext.SaveChangesAsync(); 

                    // 3. log activity
                    _appDbContext.ActivityLogs.Add(new ActivityLog
                    {
                        UserId = req.UserId,
                        LoanTransactionId = txn.Id, // حالا Id داریم
                        Action = "Loan approved",
                        Timestamp = DateTime.Now
                    });
                }
                else if (newStatus == "Rejected")
                {
                    _appDbContext.ActivityLogs.Add(new ActivityLog
                    {
                        UserId = req.UserId,
                        Action = "Loan rejected",
                        Timestamp = DateTime.Now
                    });
                }

                req.Status = newStatus;

                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

    }
}
