using System;
namespace LibrarySystem.Infrastructure.ModelDto.AdminPageDto
{
    public class GetAllUsersOutput
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime SubscriptionTime { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
