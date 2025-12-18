using Microsoft.EntityFrameworkCore;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;

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
    }
}
