using LibrarySystem.Data;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
                    BookTitle = r.Book.Title,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                })
                .ToListAsync();

        }
    }
}
