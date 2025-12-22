using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;

namespace UnimarFrontend.Dominio.Entidades
{
    public class BookGoogleDrive : Entity
    {
        [MaxLength(255)]
        public string GoogleDriveId { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
