using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;
using LibrarySystem.Web.Models;
using Microsoft.AspNet.Identity;

namespace LibrarySystem.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }


        // GET: Member
        public async Task<ActionResult> Index()
        {
            var books = await _memberServices.GetListAdmirableBooks();

            MemberDashboardViewModel viewModel = new MemberDashboardViewModel();
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

                var userId = User.Identity.GetUserId(); // اگر از ASP.NET Identity استفاده می‌کنید

                var memberLoanRequestDto = new MemberLoanRequestDto
                {
                    BookId = vm.BookId,
                    UserId = 2
                };

                await _memberServices.SubmitLoanRequest(memberLoanRequestDto);

                return Content("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(400);
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

    }
}