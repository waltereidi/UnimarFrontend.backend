
using System.ComponentModel.DataAnnotations;
using UnimarFrontend.backend.ValueObjects;

namespace UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades
{
    public class User : Entity
    {
        public  EmailVO Email { get; set; }
        
        public  PasswordVO PasswordHash { get; set; }
        
        public  string Nome { get; set; }
    }
}
