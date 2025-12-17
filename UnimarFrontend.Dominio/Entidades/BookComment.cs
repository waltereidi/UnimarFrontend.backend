namespace UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades
{
    public class BookComment : Entity
    {
        public required int BookId { get; set; }
        public required string Comment { get; set; }
        
    }
}
