
using System.ComponentModel.DataAnnotations;
using UnimarFrontend.backend.ValueObjects;

namespace UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades
{
    public class User : Entity
    {
        public required EmailVO Email { get; set; }
        
        public required PasswordVO PasswordHash { get; set; }
        
        public required string Nome { get; set; }
    }
}
