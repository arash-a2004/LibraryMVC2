using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;

namespace LibrarySystem.Infrastructure.Infra
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _appDbContext;

        public MemberRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<List<GetListAdmirableBooksDto>> GetListAdmirableBooks()
        {
            return await _appDbContext.Books
                .Where(e => e.IsAvailable)
                .Select(e => new GetListAdmirableBooksDto()
                {
                    Id = e.Id,
                    Author = e.Author,
                    Title = e.Title
                })
                .ToListAsync();
        }

        public async Task SubmitLoanRequest(MemberLoanRequestDto input)
        {
            var userId = input.UserId;
            var availablity = await _appDbContext.LoanRequests.AnyAsync(e => e.UserId == userId && e.BookId == input.BookId);
            if (availablity)
                throw new System.Exception();

            var request = new LoanRequest
            {
                UserId = userId,
                BookId = input.BookId,
                RequestDate = DateTime.Now,
                Status = "BackgroundCheck",
                User = await _appDbContext.Users.FindAsync(userId),
                Book = await _appDbContext.Books.FindAsync(input.BookId),
            };


            var book = await _appDbContext.Books.FindAsync(input.BookId);
            book.IsAvailable = false;

            _appDbContext.ActivityLogs.Add(new ActivityLog
            {
                UserId = request.UserId,
                LoanTransactionId = null,
                Action = $"Requested loan for BookId={input.BookId}",
                Timestamp = DateTime.Now
            });

            _appDbContext.LoanRequests.Add(request);

            await _appDbContext.SaveChangesAsync();

        }

        public async Task<List<ModelDto.MemberDto.LoanRequestListDto>> LoanrequestList(int userId)
        {
            var res = await _appDbContext.LoanRequests
                .Include(x => x.Book)
                .Include(x => x.LoanTransaction)
                .Where(e => e.UserId == userId)
                .Where(e => (e.LoanTransaction == null) || (e.LoanTransaction != null && e.LoanTransaction.ReturnDate == null))
                .Select(e => new ModelDto.MemberDto.LoanRequestListDto()
                {
                    BookId = e.BookId,
                    RequestDate = e.RequestDate,
                    Status = e.Status,
                    BookTitle = e.Book.Title,
                    Id = e.Id,
                })
                .ToListAsync();

            return res;
        }

        public async Task DeleteLoanRequest(int id)
        {
            try
            {
                var loanrequest = await _appDbContext.LoanRequests.FindAsync(id);

                if (loanrequest != null)
                {
                    _appDbContext.LoanRequests.Remove(loanrequest);
                }

                await _appDbContext.SaveChangesAsync();


                var book = await _appDbContext.Books.Where(e => e.Id == loanrequest.BookId).FirstAsync();

                book.IsAvailable = true;

                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task ReturnBookAsync(int bookId)
        {
            var loantranactionId = _appDbContext.Books
                .Include(e => e.LoanTransactions)
                .Where(e => e.Id == bookId)
                .First().LoanTransactions
                .Last().Id;

            var txn = await _appDbContext.LoanTransactions
                .Include(t => t.Book)
                .Include(t => t.Fine)
                .FirstOrDefaultAsync(t => t.Id == loantranactionId);

            if (txn == null)
                throw new Exception("Loan transaction not found.");

            if (txn.ReturnDate != null)
                throw new Exception("This book has already been returned.");

            // 1. Set ReturnDate
            txn.ReturnDate = DateTime.Now;

            // 2. Calculate fine
            const int allowedDays = 14; // e.g. 14 days loan period
            const decimal dailyFineRate = 500; // example fine per day

            var daysBorrowed = (txn.ReturnDate.Value - txn.LoanDate).Days;
            int daysLate = daysBorrowed - allowedDays;

            if (daysLate > 0)
            {
                decimal fineAmount = daysLate * dailyFineRate;

                if (txn.Fine == null)
                {
                    // Create new fine
                    txn.Fine = new Fine
                    {
                        DaysLate = daysLate,
                        FineAmount = fineAmount,
                        IsPaid = false,
                        PaymentDate = null,
                        LoanTransactionId = txn.Id
                    };
                    _appDbContext.fines.Add(txn.Fine);
                }
                else
                {
                    // Update existing fine
                    txn.Fine.DaysLate = daysLate;
                    txn.Fine.FineAmount = fineAmount;
                    txn.Fine.IsPaid = false;
                    txn.Fine.PaymentDate = null;
                }
            }
            else if (txn.Fine != null)
            {
                // No late, clear any existing fine
                _appDbContext.fines.Remove(txn.Fine);
                txn.Fine = null;
            }

            // 3. Mark book as available
            txn.Book.IsAvailable = true;

            // 4. Log the return
            _appDbContext.ActivityLogs.Add(new ActivityLog
            {
                UserId = txn.UserId,
                LoanTransactionId = txn.Id,
                Action = "Book returned",
                Timestamp = DateTime.Now
            });

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<GetListAdmirableBooksDto>> MyBooks(int userId)
        {
            return await _appDbContext.LoanRequests
                .Include(x => x.Book)
                .Where(e => e.UserId == userId)
                .Where(r => r.Status == "Approved"
                     && r.LoanTransaction != null
                     && r.LoanTransaction.ReturnDate == null)
                    .Select(e => new GetListAdmirableBooksDto()
                    {
                        Id = e.BookId,
                        Author = e.Book.Author,
                        Title = e.Book.Title
                    })
                .ToListAsync();

        }

        public List<UserFineCombinedDto> GetAllFinesForUser(int userId)
        {
            try
            {
                var returned = GetReturnedBookFinesForUser(userId);
                var unreturned = GetUnreturnedBookFinesForUser(userId);
                return returned.Concat(unreturned).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<UserFineCombinedDto>();
            }
        }

        protected List<UserFineCombinedDto> GetReturnedBookFinesForUser(int userId)
        {
            var returnedFines = _appDbContext.LoanTransactions
                .Where(lt => lt.ReturnDate != null && lt.Fine != null && lt.UserId == userId)
                .Select(lt => new UserFineCombinedDto
                {
                    UserId = lt.UserId,
                    LoanTransactionId = lt.Id,
                    BookTitle = lt.Book.Title,
                    LoanDate = lt.LoanDate,
                    ReturnDate = lt.ReturnDate,
                    DaysLate = lt.Fine.DaysLate,
                    FineAmount = lt.Fine.FineAmount,
                    IsPaid = lt.Fine.IsPaid,
                    PaymentDate = lt.Fine.PaymentDate
                })
                .ToList();

            return returnedFines;
        }

        protected List<UserFineCombinedDto> GetUnreturnedBookFinesForUser(int userId)
        {
            var today = DateTime.Now;
            const int AllowedLoanDays = 14;
            const decimal FinePerDay = 1000;

            var unreturnedLoans = _appDbContext.LoanTransactions
                .Where(lt => lt.ReturnDate == null && lt.UserId == userId)
                .Include(lt => lt.Book) // make sure Book is loaded
                .ToList() // move to memory (LINQ to Objects)
                .Where(lt => (DateTime.Now - lt.LoanDate.AddDays(AllowedLoanDays)).Days > 0)
                .Select(lt => new UserFineCombinedDto
                {
                    UserId = lt.UserId,
                    LoanTransactionId = lt.Id,
                    BookTitle = lt.Book.Title,
                    LoanDate = lt.LoanDate,
                    ReturnDate = null,
                    DaysLate = (DateTime.Now - lt.LoanDate.AddDays(AllowedLoanDays)).Days,
                    FineAmount = (DateTime.Now - lt.LoanDate.AddDays(AllowedLoanDays)).Days * FinePerDay,
                    IsPaid = null,
                    PaymentDate = null
                })
                .ToList();

            return unreturnedLoans;
        }

    }
}
