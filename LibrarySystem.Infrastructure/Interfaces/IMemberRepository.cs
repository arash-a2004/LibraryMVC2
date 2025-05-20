using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<GetListAdmirableBooksDto>> GetListAdmirableBooks();
        Task SubmitLoanRequest(MemberLoanRequestDto input);
        Task<List<ModelDto.MemberDto.LoanRequestListDto>> LoanrequestList(int userId);
        Task DeleteLoanRequest(int id);
        Task ReturnBookAsync(int loanTransactionId);
        Task<List<GetListAdmirableBooksDto>> MyBooks(int userId);
        List<UserFineCombinedDto> GetAllFinesForUser(int userId);
    }
}
