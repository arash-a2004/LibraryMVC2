using System.Collections.Generic;

namespace LibrarySystem.Web.Models
{
    public class LibrarianDashboardViewModel
    {
        public BooksViewModel booksViewModel { get; set; }
        public List<PendingRequestViewModel> pendingRequestViewModels { get; set; } = new List<PendingRequestViewModel>();
        public UsersViewModel UsersModel { get; set; } = new UsersViewModel();

        //public IEnumerable<UserViewModel> Users { get; set; }
    }

}