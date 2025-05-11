namespace LibrarySystem.Infrastructure.ModelDto.AccountingDto
{
    public class UserSignUpDto
    {
        public string Username { get; set; }
        public string Password { get; set; } 

    }

    public class UserSignInDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
