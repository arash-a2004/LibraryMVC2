﻿namespace LibrarySystem.Infrastructure.BCryptServices
{
    public class PasswordHashingServices
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

    }
}
