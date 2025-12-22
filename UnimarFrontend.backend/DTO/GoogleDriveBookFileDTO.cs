using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;

namespace UnimarFrontend.backend.DTO
{
    public class GoogleDriveBookFileDTO
    {
        public record BookDrive(Book book , string driveId);
        public List<BookDrive> BooksDrive { get; private set; }
        public GoogleDriveBookFileDTO()
        { }
        public void AddBookDrive(IList<Google.Apis.Drive.v3.Data.File> files)
        {
            var books  = files.Select(s => {
                var book = new Book()
                {
                    Title = s.Name,
                    ThumNail = ExchangeExtension(s.Name),
                    CreatedAt = s.CreatedTime == null ? DateTime.MinValue : s.CreatedTime.Value,
                    DriveUrl = s.WebContentLink

                };
                return new BookDrive(book , s.Id);
            })
            .ToList();

            if(BooksDrive == null )
            {
                BooksDrive = books;
            } else
                BooksDrive.AddRange(books);
        }
        public string ExchangeExtension(string str)
            => str.Replace(".pdf", ".jpg");
    }
}
