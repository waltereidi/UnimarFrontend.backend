using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;

namespace UnimarFrontend.Dominio.Entidades
{
    public class FileStorage : Entity
    {
        [MaxLength(255)]
        public string FilePath { get; set; }
        [MaxLength(255)]
        public string OriginalFileName { get; set; }

    }
}
