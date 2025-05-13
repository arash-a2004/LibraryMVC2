using LibrarySystem.Domain.Models.DbModels;

namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public sealed class GetAllUsersInput
    {
        public string Username { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
        public int SkipCount { get; set; } = 0;
        public int MaxResult { get; set; } = 10;
    }
}
