using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Application.Interfaces
{
    public interface IFineCheckerServices
    {
        Task<List<LoanRequestListDto>> BackGroundCheckList();
    }
}
