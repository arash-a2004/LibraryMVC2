using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Models.DbModels;
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

        public List<UserFineCombinedDto> GetAllFinesForUser(int userId)
        {
            return fineCheckerRepository.GetAllFinesForUser(userId);
        }

        public async Task Reject(int Id)
        {
            await fineCheckerRepository.Reject(Id);
        }
        public async Task Approve(int Id)
        {
            await fineCheckerRepository.Approve(Id);
        }

        public async Task<LoanRequest> GetLoanrequestDetail(int Id)
        {
            return await fineCheckerRepository.GetLoanrequestDetail(Id);    
        }

    }
}
