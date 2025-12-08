namespace UnimarFrontend.backend.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string ThumNail { get; set; }
    }
}
