namespace UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades
{
    public class BookComment : Entity
    {
        public  int BookId { get; set; }
        public virtual List<BookComment> BookComments { get; set; }
        public virtual Book Book { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
