﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var pendingList = await _librarianService.GetPendingRequests();
                GetAllUsersInput booksInput = new GetAllUsersInput();   
                var users = await _librarianService.GetAllUser(booksInput);
                var model = new UsersViewModel
                {
                    Username = booksInput.Username,
                    IsActive = booksInput.IsActive,
                    Users = users
                };
                librarianDashboardViewModel.booksViewModel = new BooksViewModel();
                librarianDashboardViewModel.booksViewModel.Books = books;
                librarianDashboardViewModel.UsersModel = model;
                librarianDashboardViewModel.pendingRequestViewModels = pendingList.Select(e => new PendingRequestViewModel()
                {
                    Id = e.Id,
                    RequestDate = e.RequestDate,
                    BookTitle = e.BookTitle,
                    Status = e.Status,
                    Username = e.Username
                }).ToList();

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


        [HttpPost, ValidateAntiForgeryToken]
        //[HttpPost]
        public async Task<ActionResult> ChangePendingStatus(int id, string newStatus)
        {
            try
            {
                await _librarianService.ChangePendingStatus(id, newStatus);

                return await GetPendingRequests();

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);    
                return await GetPendingRequests();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPendingRequests()
        {
            var a = await _librarianService.GetPendingRequests();

            var b = a.Select(e => new PendingRequestViewModel()
            {
                BookTitle = e.BookTitle,
                RequestDate = e.RequestDate,
                Id = e.Id,
                Status = e.Status,
                Username = e.Username
            }).ToList();

            return PartialView("_PendingRequestListPartial", b);

        }


        public async Task<ActionResult> UserDetails(int id)
        {
            var user = await _librarianService.GetUserDetailById(id);
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


        [HttpGet]
        public async Task<PartialViewResult> GetFilteredUsers(GetAllUsersInput input)
        {
            try
            {
                var users = await _librarianService.GetAllUser(input);
                var model = new UsersViewModel
                {
                    Username = input.Username,
                    IsActive = input.IsActive,
                    Users = users
                };

                return PartialView("_UserListPartial", model);
            }
            catch (System.Exception ex)
            {
                return PartialView("_UserListPartial", new UsersViewModel());

            }
        }


    }
}