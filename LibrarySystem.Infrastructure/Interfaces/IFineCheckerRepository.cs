using LibrarySystem.Infrastructure.ModelDto.FineChecker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibrarySystem.Infrastructure.Interfaces
{
    public interface IFineCheckerRepository
    {
        Task<List<LoanRequestListDto>> BackGroundCheckList();
    }
}
