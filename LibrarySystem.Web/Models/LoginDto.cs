namespace LibrarySystem.Web.Models
{
    public class UserSignUpDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserSignInDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}