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

            fineChecker.BackgroundCheckRequests = res.Select(e=> new LoanRequestListViewModel()
            {
                RequestDate = e.RequestDate,
                BookTitle = e.BookTitle,
                Status = e.Status,
                Id = e.Id
            }).ToList();


            return View(fineChecker);
        }
    }
}