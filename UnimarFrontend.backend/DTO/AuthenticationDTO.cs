namespace UnimarFrontend.backend.DTO
{
    public class AuthenticationDTO
    {
        public class Request
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class Response
        {
            public string JwtToken { get; set; }
        }

    }
}
