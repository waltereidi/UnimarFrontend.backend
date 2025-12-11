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
            public string Message { get; set; }
            public string Nome { get; set; }
        }

    }
}
