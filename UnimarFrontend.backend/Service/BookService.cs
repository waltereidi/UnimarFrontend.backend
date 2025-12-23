using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using UnimarFrontend.backend.DTO;
using UnimarFrontend.backend.GoogleDriveApi;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;
using UnimarFrontend.Dominio.Entidades;

namespace UnimarFrontend.backend.Service
{
    public class BookService
    {
        private readonly AppDbContext _dbContext;

        public BookService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public DateTime GetLastBook()
        {
            Console.WriteLine("===========================QUARTZ===============================");
            Console.WriteLine("GetLastBook");
            if (!_dbContext.Books.Any())
                return DateTime.Now.AddYears(-5);

            Console.WriteLine("GetLastBook 29");
            return _dbContext.Books
                .OrderByDescending(o => o.CreatedAt)
                .First()
                .CreatedAt;
        }
        public async Task<int> AddBookRange(DateTime lastBookTime )
        {
            Console.WriteLine("===========================QUARTZ===============================");
            Console.WriteLine("AddBookRange");
            var dto = GetFilesFromDrive(lastBookTime);

            var books = dto.BooksDrive.Select(s => s.book);
            Console.WriteLine("AddBookRange42");
            await _dbContext.Books.AddRangeAsync(books );
            await _dbContext.SaveChangesAsync();

            var bookDrives = books.Join( dto.BooksDrive , 
                b=> b.Title, 
                d => d.book.Title, 
                (b , d ) => new BookGoogleDrive
                {
                    BookId = b.Id , 
                    GoogleDriveId = d.driveId,
                });
            Console.WriteLine("AddBookRange53");
            await _dbContext.BookGoogleDrive.AddRangeAsync(bookDrives);

            var result = await _dbContext.SaveChangesAsync();

            return result;
        }

        private GoogleDriveBookFileDTO GetFilesFromDrive(DateTime lastBookTime )
        {
            Console.WriteLine("===========================QUARTZ===============================");
            Console.WriteLine("GetFilesFromDrive");

            var drive = new GoogleDriveRead();
            var dto = new GoogleDriveBookFileDTO();
            bool isEmpty = false;
            while(!isEmpty)
            {
                try
                {
                    var files = drive.GetDriveFilesFromCreationDate(lastBookTime, DateTime.Now);

                    isEmpty = (files == null || files.Count() == 0);
                    if (isEmpty)
                        break;
                    dto.AddBookDrive(files);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                
            }
            return dto;
        }

        public List<Book> GetBookWithouthThumbNail()
        {
            var result = _dbContext.Books
                .Where(x => x.BookFileStorages == null)
                .Take(20)
                .ToList();
            return result;
        }

        public void GenerateThumbNail(List<Book> result)
        {
            //result.ForEach(f => {
            //    var pdf = f.DownloadBook(f.);
            
            //});
        }
    }
}
