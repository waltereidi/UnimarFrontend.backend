using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;

namespace UnimarFrontend.Dominio.Entidades
{
    public class BookFileStorage : Entity
    {
        public int FileStorageId { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
