using UnimarFrontend.backend.Models;
namespace UnimarFrontend.backend.DTO
{
    public class CreateUserDTO
    {
        public class Request
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public User ToUserEntity()
            => new User
                {
                    Email = new ValueObjects.EmailVO(this.Email),
                    PasswordHash = new ValueObjects.PasswordVO(this.Password),
                    Nome = Name
                };
        }
    }
}
