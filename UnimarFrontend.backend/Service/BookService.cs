using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using UnimarFrontend.backend.DTO;
using UnimarFrontend.backend.GoogleDriveApi;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;
using UnimarFrontend.Dominio.Entidades;
using static System.Reflection.Metadata.BlobBuilder;

namespace UnimarFrontend.backend.Service
{
    public class BookService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public BookService(AppDbContext dbContext  )
        {
            _dbContext = dbContext;


            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();


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
            var dto = GetFilesFromDrive(lastBookTime);

            var books = dto.BooksDrive.Select(s => s.book);
            if (books == null || books.Count() == 0)
                return 0;

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
                    Console.WriteLine($"GetFilesFromDrive lastbookTime = {lastBookTime.ToString()}");
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

        public Book GetBookWithouthThumbNail()
        {
            var result = _dbContext.Books
                .Where(b => !b.BookFileStorages.Any())
                .Include(b => b.BookGoogleDrives)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefault();

            return result;
        }

        public void GenerateThumbNail(Book book)
        {
            var config = _configuration.GetSection("FileStorage").GetChildren();
            var path = config.ElementAt(0).Value;

            var pathThumbnail = config.ElementAt(1).Value;
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(pathThumbnail))
            {
                Console.WriteLine("PdfPig não está configurado");
                throw new Exception($"PdfPig não está configurado {nameof(path)}");
            }
            
            var pdfPigService = new PdfPigThumbNail();

            var bgd = book.BookGoogleDrives.FirstOrDefault();
            var gservice = new GoogleDriveDownload(bgd.GoogleDriveId, book.Title, path);
            var file = gservice.Start();
            if(file.Exists)
            {
                var fileStorage = new FileStorage
                {
                    FilePath = file.FullName,
                    OriginalFileName = file.Name,
                    CreatedAt = DateTime.Now,
                };

                _dbContext.FileStorage.Add(fileStorage);
                _dbContext.SaveChanges();

                var bookFileStorage = new BookFileStorage
                {
                    BookId = book.Id,
                    FileStorageId = fileStorage.Id,
                };
                _dbContext.BookFileStorages.Add(bookFileStorage);
                _dbContext.SaveChanges();

            }
            var thumb= pdfPigService.GetPdfPage(file, new DirectoryInfo(pathThumbnail));
            if(thumb.Exists)
            {
                var fileStorageThumb = new FileStorage
                {
                    FilePath = thumb.FullName,
                    OriginalFileName = thumb.Name,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.FileStorage.Add(fileStorageThumb);
                _dbContext.SaveChanges();

                var bookFileStorageThumb = new BookFileStorage
                {
                    BookId = book.Id,
                    FileStorageId = fileStorageThumb.Id,
                };
                _dbContext.BookFileStorages.Add(bookFileStorageThumb);
                _dbContext.SaveChanges();
            }


        }

        public List<string> GetTest()
        {
            var config = _configuration.GetSection("FileStorage").GetChildren();
            var path = config.ElementAt(0).Value;
            
            var pathThumbnail = config.ElementAt(1).Value;
            return new List<string>() { path, pathThumbnail };
        }
    }
}
