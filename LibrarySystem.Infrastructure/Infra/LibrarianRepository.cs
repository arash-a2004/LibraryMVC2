using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LibrarySystem.Data;
using LibrarySystem.Domain.Models.DbModels;
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
                .Include(u=>u.LoanTransactions)
                .Where(u => u.Id == bookId)
                .FirstOrDefaultAsync();

            if (books is null)
                throw new KeyNotFoundException("there is no Book with this Id");

            if (books.LoanTransactions.Any())
                throw new System.Exception("can not delete this book becaues there are some loantransaction on book");
        }

    }
}
