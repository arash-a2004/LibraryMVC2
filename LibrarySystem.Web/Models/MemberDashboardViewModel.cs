using System.Collections.Generic;

namespace LibrarySystem.Web.Models
{
    public class MemberDashboardViewModel
    {
        public List<LoanBookViewModels> books { get; set; } = new List<LoanBookViewModels>();
        public List<LoanRequestListViewModel> loanRequestListViewModels { get; set; } = new List<LoanRequestListViewModel>();
    }

    public class LoanBookViewModels
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
        public CreateLoanRequestViewModel RequestBorrowModel { get; set; } = new CreateLoanRequestViewModel();

    }
}