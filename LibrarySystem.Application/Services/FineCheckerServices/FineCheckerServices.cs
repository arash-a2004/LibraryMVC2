using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.FineChecker;

namespace LibrarySystem.Application.Services.FineCheckerServices
{
    public class FineCheckerServices : IFineCheckerServices
    {
        private readonly IFineCheckerRepository fineCheckerRepository;

        public FineCheckerServices(IFineCheckerRepository fineCheckerRepository)
        {
            this.fineCheckerRepository = fineCheckerRepository;
        }
        public async Task<List<LoanRequestListDto>> BackGroundCheckList()
        {
            return await fineCheckerRepository.BackGroundCheckList();
        }

    }
}
