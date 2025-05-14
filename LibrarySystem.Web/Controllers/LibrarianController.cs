using System.Threading.Tasks;
using System.Web.Mvc;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services.AdminServices;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Infrastructure.ModelDto.LibrarianPageDto;
using LibrarySystem.Web.Models;

namespace LibrarySystem.Web.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly ILibrarianService _librarianService;

        public LibrarianController(ILibrarianService librarianService)
        {
            _librarianService = librarianService;
        }

        // GET: Librarian
        public async Task<ActionResult> Index()
        {
            GetAllBooksInput b = new GetAllBooksInput();
            LibrarianDashboardViewModel librarianDashboardViewModel = new LibrarianDashboardViewModel();
            try
            {
                var books = await _librarianService.GetAllBook(b);
                librarianDashboardViewModel.booksViewModel = new BooksViewModel();
                librarianDashboardViewModel.booksViewModel.Books = books;

                return View(librarianDashboardViewModel);
            }
            catch (System.Exception ex)
            {
                return View(librarianDashboardViewModel);
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> GetFilteredBooks(GetAllBooksInput input)
        {
            try
            {
                var books = await _librarianService.GetAllBook(input);
                var model = new BooksViewModel()
                {
                    Books = books,
                    Title = input.Title,
                    Author = input.Author,
                    IsAvailable = (input.IsAvailable.HasValue) ? input.IsAvailable.Value : true,
                    SkipCount = input.SkipCount,
                    MaxResult = input.MaxResult,
                };

                return PartialView("_LibrarianBookListPartial", model);
            }
            catch (System.Exception ex)
            {
                return PartialView("_LibrarianBookListPartial", new BooksViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBook(string Title, string Author)
        {
            try
            {
                CreateBookDto createBookDto = new CreateBookDto();

                createBookDto.Title = Title;
                createBookDto.Author = Author;

                await _librarianService.CreateNewBook(createBookDto);

                return new HttpStatusCodeResult(200);
            }
            catch (System.Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                await _librarianService.DeleteBook(id);
                return new HttpStatusCodeResult(200);
            }
            catch (System.Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }

    }
}