using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services.FineCheckerServices;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;
using LibrarySystem.Web.CustomAttribute;
using LibrarySystem.Web.Models;

namespace LibrarySystem.Web.Controllers
{
    [CustomAuthorize]
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;
        protected int CurrentUserId => (int)HttpContext.Items["UserId"];
        protected string CurrentUserRole => HttpContext.Items["UserRole"].ToString();

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }


        // GET: Member
        public async Task<ActionResult> Index()
        {
            var books = await _memberServices.GetListAdmirableBooks();
            var res = await _memberServices.LoanrequestList(CurrentUserId);

            var Mybooks = await _memberServices.MyBooks(CurrentUserId);

            List<LoanBookViewModels> loanBookViewModels = new List<LoanBookViewModels>();
            loanBookViewModels = Mybooks.Select(e => new LoanBookViewModels()
            {
                Author = e.Author,
                Id = e.Id,
                Title = e.Title
            }).ToList();

            var viewRes = res.Select(e => new LoanRequestListViewModel()
            {
                BookId = e.BookId,
                Status = e.Status,
                BookTitle = e.BookTitle,
                RequestDate = e.RequestDate,
                Id = e.Id
            }).ToList();

            MemberDashboardViewModel viewModel = new MemberDashboardViewModel();
            viewModel.loanRequestListViewModels = viewRes;
            viewModel.myBooks = loanBookViewModels;
            viewModel.books = books.Select(e => new LoanBookViewModels()
            {
                Author = e.Author,
                Id = e.Id,
                Title = e.Title
            }).ToList();

            return View(viewModel);
        }

        public async Task<ActionResult> GetListAdmirableBooks()
        {
            var books = await _memberServices.GetListAdmirableBooks();

            List<LoanBookViewModels> loanBookViewModels = new List<LoanBookViewModels>();
            loanBookViewModels = books.Select(e => new LoanBookViewModels()
            {
                Author = e.Author,
                Id = e.Id,
                Title = e.Title
            }).ToList();

            return PartialView("_AdmirableBooksPartial", loanBookViewModels);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitLoanRequest(CreateLoanRequestViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_RequestLoanPartial", vm);
                }

                var userId = CurrentUserId;

                var memberLoanRequestDto = new MemberLoanRequestDto
                {
                    BookId = vm.BookId,
                    UserId = userId
                };

                await _memberServices.SubmitLoanRequest(memberLoanRequestDto);

                return Content("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return PartialView("_RequestLoanPartial", vm);
            }
        }

        [HttpGet]
        public ActionResult RequestLoanForm(int bookId, string bookTitle)
        {
            var vm = new CreateLoanRequestViewModel
            {
                BookId = bookId,
                BookTitle = bookTitle
            };

            return PartialView("_RequestLoanPartial", vm);
        }

        public async Task<ActionResult> ViewListLoanRequests()
        {
            var res = await _memberServices.LoanrequestList(CurrentUserId);
            var viewRes = res.Select(e => new LoanRequestListViewModel()
            {
                BookId = e.BookId,
                Status = e.Status,
                BookTitle = e.BookTitle,
                RequestDate = e.RequestDate,
                Id = e.Id
            }).ToList();

            return PartialView("_LoanRequestsListPartial", viewRes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelLoanRequest(int id)
        {
            try
            {
                await _memberServices.DeleteLoanRequest(id);
                return Content("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(400);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ReturnBookAsync(int bookId)
        {
            try
            {
                await _memberServices.ReturnBookAsync(bookId);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false });

            }
        }

        public async Task<ActionResult> GetMyBooks()
        {
            var books = await _memberServices.MyBooks(CurrentUserId);

            List<LoanBookViewModels> loanBookViewModels = new List<LoanBookViewModels>();
            loanBookViewModels = books.Select(e => new LoanBookViewModels()
            {
                Author = e.Author,
                Id = e.Id,
                Title = e.Title
            }).ToList();

            return PartialView("_MyBooksPartial", loanBookViewModels);
        }


        // GET: /FineChecker/Details/5
        [HttpGet]
        public ActionResult FineDetails(int id)
        {
            var fines = _memberServices.GetAllFinesForUser(CurrentUserId);

            var returned = fines
                .Where(f => f.IsReturned)
                .Select(e => new UserFineCombinedViewModel()
                {
                    UserId = e.UserId,
                    DaysLate = e.DaysLate,
                    LoanDate = e.LoanDate,
                    PaymentDate = e.PaymentDate,
                    BookTitle = e.BookTitle,
                    ReturnDate = e.ReturnDate,
                    FineAmount = e.FineAmount,
                    IsPaid = e.IsPaid,
                    LoanTransactionId = e.LoanTransactionId,
                }).ToList();

            var unreturned = fines
                .Where(f => !f.IsReturned)
                .Select(e => new UserFineCombinedViewModel()
                {
                    UserId = e.UserId,
                    DaysLate = e.DaysLate,
                    LoanDate = e.LoanDate,
                    PaymentDate = e.PaymentDate,
                    BookTitle = e.BookTitle,
                    ReturnDate = e.ReturnDate,
                    FineAmount = e.FineAmount,
                    IsPaid = e.IsPaid,
                    LoanTransactionId = e.LoanTransactionId,
                }).ToList();

            var model = new UserFineDetailsViewModel
            {
                //UserId = request.UserId,
                //LoanRequestId = request.Id,
                //RequestedBook = request.Book.Title,
                //RequestDate = request.RequestDate,
                ReturnedFines = returned,
                UnreturnedFines = unreturned
            };

            return PartialView("_FineDetails", model);

        }

    }
}