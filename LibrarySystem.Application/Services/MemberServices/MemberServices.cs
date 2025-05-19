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
    }
}
