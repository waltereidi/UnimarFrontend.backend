namespace UnimarFrontend.backend.ValueObjects
{
    using System.Text.RegularExpressions;

    public sealed class EmailVO
    {
        public string Value { get; }

        private EmailVO() { } // EF Core

        public EmailVO(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email não pode ser vazio.");

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Email inválido.");

            Value = value.Trim().ToLowerInvariant();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
            => obj is EmailVO other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
