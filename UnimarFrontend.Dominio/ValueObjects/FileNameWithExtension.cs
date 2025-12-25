using System;
using System.Collections.Generic;
using System.Text;

namespace UnimarFrontend.Dominio.ValueObjects
{
    public sealed class FileNameWithExtension : IEquatable<FileNameWithExtension>
    {
        public string Value { get; }

        public FileNameWithExtension(string fileName, string newExtension)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (string.IsNullOrWhiteSpace(newExtension))
                throw new ArgumentException("Extension cannot be null or empty.", nameof(newExtension));

            var normalizedExtension = newExtension.StartsWith(".")
                ? newExtension
                : $".{newExtension}";

            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);

            Value = $"{fileNameWithoutExtension}{normalizedExtension}";
        }

        public override string ToString() => Value;

        #region Equality

        public bool Equals(FileNameWithExtension? other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
            => obj is FileNameWithExtension other && Equals(other);

        public override int GetHashCode()
            => Value.GetHashCode();

        #endregion
    }
}
