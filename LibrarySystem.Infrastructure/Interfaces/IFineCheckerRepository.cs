using LibrarySystem.Domain.Models.DbModels;
using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IFineCheckerRepository
    {
        Task<List<LoanRequestListDto>> BackGroundCheckList();
        List<UserFineCombinedDto> GetAllFinesForUser(int userId);
        Task Reject(int Id);
        Task Approve(int Id);
        Task<LoanRequest> GetLoanrequestDetail(int Id);
    }
}
