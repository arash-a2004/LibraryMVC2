using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.FineChecker;

namespace LibrarySystem.Infrastructure.Infra
{
    public class FineCheckerRepository : IFineCheckerRepository
    {
        private readonly AppDbContext _dbContext;
        public FineCheckerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LoanRequestListDto>> BackGroundCheckList()
        {
            return await _dbContext.LoanRequests
                .Where(r => r.Status == "BackgroundCheck")
                .Include(r => r.User)
                .Include(r => r.Book)
                .Select(r => new LoanRequestListDto
                {
                    Id = r.Id,
                    Username = r.User.Username,
                    UserId = r.UserId,
                    BookTitle = r.Book.Title,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                })
                .ToListAsync();

        }

        public async Task Approve(int Id)
        {
            var req = await _dbContext.LoanRequests.FindAsync(Id);
            if (req == null)
                throw new KeyNotFoundException(nameof(req));

            req.Status = "Pending";
            await _dbContext.SaveChangesAsync();
        }

        public async Task Reject(int Id)
        {
            var req = await _dbContext.LoanRequests.FindAsync(Id);
            if (req == null)
                throw new KeyNotFoundException(nameof(req));

            req.Status = "Rejected";
            await _dbContext.SaveChangesAsync();
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
            var returnedFines = _dbContext.LoanTransactions
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

            var unreturnedLoans = _dbContext.LoanTransactions
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

        public async Task<LoanRequest> GetLoanrequestDetail(int Id)
        {
            return await _dbContext.LoanRequests
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == Id);
        }

    }
}
