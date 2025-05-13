using System.Collections.Generic;
using LibrarySystem.Infrastructure.ModelDto.AdminPageDto;

namespace LibrarySystem.Web.Models
{

    public class UsersViewModel
    {
        public string Username { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
        public List<GetAllUsersOutput> Users { get; set; } = new List<GetAllUsersOutput>(); 
        public int SkipCount { get; set; } = 0;
        public int MaxResult { get; set; } = 10;

    }


}