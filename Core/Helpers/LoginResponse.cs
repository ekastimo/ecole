namespace Core.Helpers
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        /// <summary>
        /// Time in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        public IdentityToken IdentityToken { get; set; }
    }

    public class RegisterResponse
    {
        public string UserId { get; set; }
    }


    public class IdentityToken
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string FullName { get; set; }

        public IdentityToken()
        {
            FullName = $"{FirstName} {LastName}";
        }
    }
}