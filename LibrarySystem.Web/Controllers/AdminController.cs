using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;
using LibrarySystem.Web.Models;

namespace LibrarySystem.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<ActionResult> Index()
        {
            GetAllUsersInput a = new GetAllUsersInput();
            GetAllBooksInput b = new GetAllBooksInput();
            var users = await _adminService.GetAllUser(a);
            var books = await _adminService.GetAllBook(b);

            AdminDashboardViewModel dashboardViewModel = new AdminDashboardViewModel();

            dashboardViewModel.UsersModel = new UsersViewModel();
            dashboardViewModel.BooksModel = new BooksViewModel();

            dashboardViewModel.UsersModel.Users = users;
            dashboardViewModel.BooksModel.Books = books;

            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<PartialViewResult> GetFilteredUsers(GetAllUsersInput input)
        {
            try
            {
                var users = await _adminService.GetAllUser(input);
                var model = new UsersViewModel
                {
                    Username = input.Username,
                    IsActive = input.IsActive,
                    Users = users
                };

                return PartialView("_UserListPartial", model);
            }
            catch(System.Exception ex)
            {
                return PartialView("_UserListPartial", new UsersViewModel());

            }
        }
        
        [HttpGet]
        public async Task<PartialViewResult> GetFilteredBooks(GetAllBooksInput input)
        {
            try
            {
                var books = await _adminService.GetAllBook(input);
                var model = new BooksViewModel()
                {
                    Books = books,
                    Title = input.Title,
                    Author = input.Author,
                    IsAvailable = (input.IsAvailable.HasValue) ? input.IsAvailable.Value : true,
                    SkipCount = input.SkipCount,
                    MaxResult = input.MaxResult,
                };

                return PartialView("_BookListPartial", model);
            }
            catch(System.Exception ex)
            {
                return PartialView("_BookListPartial", new BooksViewModel());
            }
        }

        public async Task<ActionResult> UserDetails(int id)
        {
            var user = await _adminService.GetUserDetailById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UserDetailViewModel
            {
                Id = user.Id,
                Username = user.Username,
                SubscriptionTime = user.SubscriptionTime,
                IsActive = user.IsActive,
                LoanRequests = user.LoanRequests.Select(lr => new Models.LoanRequests
                    {
                        BookTitle = lr.BookTitle,
                        RequestDate = lr.RequestDate,
                        Status = lr.Status
                    })
                    .ToList()
            };

            return View(model);
        }

        public async Task<ActionResult> Bookdetails(int Id)
        {
            var book = await _adminService.GetBookDetails(Id);

            if (book == null)
            {
                return HttpNotFound();
            }

            var model = new BookDetailViewModel
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
                IsAvailable = book.IsAvailable,
                transactions = book.transactions.Select(lr => new Models.transaction
                {
                    LoanDate = lr.LoanDate,
                    ReturnDate = lr.ReturnDate,
                    Username = lr.Username,
                }).ToList()
            };

            return View(model);

        }


    }
}