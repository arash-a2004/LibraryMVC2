using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Web.Models;

namespace LibrarySystem.Web.Controllers
{
    public class FineCheckerController : Controller
    {
        private readonly IFineCheckerServices _fineCheckerServices;

        public FineCheckerController(IFineCheckerServices fineCheckerServices)
        {
            _fineCheckerServices = fineCheckerServices;
        }

        // GET: FineChecker
        public async Task<ActionResult> Index()
        {
            FineCheckerDashboardViewModel fineChecker = new FineCheckerDashboardViewModel();

            var res = await _fineCheckerServices.BackGroundCheckList();

            fineChecker.BackgroundCheckRequests = res.Select(e => new LoanRequestListViewModel()
            {
                RequestDate = e.RequestDate,
                BookTitle = e.BookTitle,
                Status = e.Status,
                Id = e.Id
            }).ToList();


            return View(fineChecker);
        }

        // GET: /FineChecker/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var request = await _fineCheckerServices.GetLoanrequestDetail(id);

            var fines = _fineCheckerServices.GetAllFinesForUser(request.UserId);

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
                UserId = request.UserId,
                LoanRequestId = request.Id,
                RequestedBook = request.Book.Title,
                RequestDate = request.RequestDate,
                ReturnedFines = returned,
                UnreturnedFines = unreturned
            };

            return PartialView("_FineDetails", model);

        }

        // POST: /FineChecker/Approve/5
        [HttpPost]
        public async Task<ActionResult> Approve(int id)
        {
            await _fineCheckerServices.Approve(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> Reject(int id)
        {
            await _fineCheckerServices.Reject(id);
            return Json(new { success = true });
        }




    }
}