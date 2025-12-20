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
            if (!_dbContext.Books.Any())
                return DateTime.MinValue;

            return _dbContext.Books
                .OrderByDescending(o => o.CreatedAt)
                .First()
                .CreatedAt;
        }
        public async Task<int> AddBookRange(DateTime lastBookTime )
        {
            var dto = GetFilesFromDrive(lastBookTime);

            var books = dto.BooksDrive.Select(s => s.book);

            await _dbContext.Books.AddRangeAsync(books );

            var bookDrives = books.Join( dto.BooksDrive , 
                b=> b.Title, 
                d => d.book.Title, 
                (b , d ) => new BookGoogleDrive
                {
                    BookId = b.Id , 
                    GoogleDriveId = d.driveId,
                });
            await _dbContext.BookGoogleDrive.AddRangeAsync(bookDrives);

            var result = await _dbContext.SaveChangesAsync();

            return result;
        }

        private GoogleDriveBookFileDTO GetFilesFromDrive(DateTime lastBookTime )
        {
            var drive = new GoogleDriveRead();
            var dto = new GoogleDriveBookFileDTO();
            bool isEmpty = false;
            while(!isEmpty)
            {
                var files = drive.GetDriveFilesFromCreationDate(lastBookTime);

                isEmpty = (files == null || files.Count() == 0);
                if(isEmpty)
                    break;
                dto.AddBookDrive(files);
            }
            return dto;
        }
        
    }
}
