using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Application.Interfaces
{
    public interface IMemberServices
    {
        Task<List<GetListAdmirableBooksDto>> GetListAdmirableBooks();
        Task SubmitLoanRequest(MemberLoanRequestDto input);
        Task<List<Infrastructure.ModelDto.MemberDto.LoanRequestListDto>> LoanrequestList(int userId);
        Task DeleteLoanRequest(int id);
        Task ReturnBookAsync(int loanTransactionId);
        Task<List<GetListAdmirableBooksDto>> MyBooks(int userId);
        List<UserFineCombinedDto> GetAllFinesForUser(int userId);

    }

}
