using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Infrastructure.Interfaces;
using LibrarySystem.Infrastructure.ModelDto.MemberDto;

namespace LibrarySystem.Application.Services.MemberServices
{
    public class MemberServices : IMemberServices
    {
        private readonly IMemberRepository _memberRepository;

        public MemberServices(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }


        public async Task<List<GetListAdmirableBooksDto>> GetListAdmirableBooks()
        {
            return await _memberRepository.GetListAdmirableBooks();
        }

        public async Task SubmitLoanRequest(MemberLoanRequestDto input)
        {
            await _memberRepository.SubmitLoanRequest(input);
        }
        public async Task<List<LoanRequestListDto>> LoanrequestList(int userId)
        {
            return await _memberRepository.LoanrequestList(userId);
        }
        public async Task DeleteLoanRequest(int id)
        {
            await _memberRepository?.DeleteLoanRequest(id);
        }
        public async Task ReturnBookAsync(int loanTransactionId)
        {
            await _memberRepository.ReturnBookAsync(loanTransactionId);
        }
    }
}
