using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnimarFrontend.Dominio.Entidades
{
    public class BookGoogleDrive
    {
        [MaxLength(255)]
        public string GoogleDriveId { get; set; }
        public int BookId { get; set; }
    }
}
