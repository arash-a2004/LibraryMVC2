using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.Interfaces;
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
            try
            {
                var availablity = await _appDbContext.LoanRequests.AnyAsync(e => e.UserId == input.UserId && e.BookId == input.BookId);
                if (availablity)
                    throw new System.Exception();

                var request = new LoanRequest
                {
                    UserId = 2,
                    BookId = input.BookId,
                    RequestDate = DateTime.Now,
                    Status = "BackgroundCheck",
                    User = await _appDbContext.Users.FindAsync(2),
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
            catch (System.Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<List<LoanRequestListDto>> LoanrequestList(int userId)
        {
            return await _appDbContext.LoanRequests
                .Include(x => x.Book)
                .Where(e => e.UserId == userId)
                .Select(e => new LoanRequestListDto()
                {
                    BookId = e.BookId,
                    RequestDate = e.RequestDate,
                    Status = e.Status,
                    BookTitle = e.Book.Title,
                    Id = e.Id,
                })
                .ToListAsync();
        }

        public async Task DeleteLoanRequest(int id)
        {
            var loanrequest = await _appDbContext.LoanRequests.FindAsync(id);

            if (loanrequest != null)
            {
                _appDbContext.LoanRequests.Remove(loanrequest);
            }

            await _appDbContext.SaveChangesAsync();


            var book =await  _appDbContext.Books.Where(e => e.Id == loanrequest.BookId).FirstAsync();

            book.IsAvailable = true;

            await _appDbContext.SaveChangesAsync();
        }
    }
}
