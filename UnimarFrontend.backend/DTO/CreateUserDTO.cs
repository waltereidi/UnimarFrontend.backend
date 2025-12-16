using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.ValueObjects;
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
                    Email = new EmailVO(this.Email),
                    PasswordHash = new PasswordVO(this.Password),
                    Nome = Name
                };
        }
    }
}
