using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades
{
    public class Book : Entity
    {

        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(128)]
        public string? Author { get; set; }
        [MaxLength(18)]
        public string? ISBN { get; set; }
        [MaxLength(255)]
        public string? ThumNail { get; set; }
        [MaxLength(254)]
        public string? DriveUrl { get; set; }
        [MaxLength(10)]
        public string? FileSize { get; set; }

    }
}
