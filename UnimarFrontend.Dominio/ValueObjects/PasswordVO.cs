using System.Text;
using System.Security.Cryptography;
namespace UnimarFrontend.backend.ValueObjects
{

    public sealed class PasswordVO
    {
        public string Hash { get; }

        public PasswordVO(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Senha não pode ser vazia.");

            if (plainPassword.Length < 6)
                throw new ArgumentException("Senha deve ter pelo menos 6 caracteres.");

            Hash = HashPassword(plainPassword);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }

        public bool Verify(string plainPassword)
            => Hash == HashPassword(plainPassword);

        public override bool Equals(object? obj)
            => obj is PasswordVO other && Hash == other.Hash;

        public override int GetHashCode() => Hash.GetHashCode();
        public static implicit operator string(PasswordVO password)
        {
            return password.Hash;
        }
    }
}
