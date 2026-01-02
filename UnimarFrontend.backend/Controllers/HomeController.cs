using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Linq;
using UnimarFrontend.backend.Service;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;
using Serilog;
namespace UnimarFrontend.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetBooks")]
        public IEnumerable<Book> GetBooks(int page)
        {
            try
            {
                var result = _context.Books
                .Include(b => b.BookComments)
                .Include(b => b.BookFileStorages)
                .Skip(page * 5)
                .Take(5)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
                result.ForEach(b =>
                {
                    if(b.ThumNail.Contains(".jpg"))
                        b.ThumNail = b.ThumNail.Replace(".jpg", ".png.png");
                    else if(b.ThumNail.Contains(".png"))
                        b.ThumNail = b.ThumNail.Replace(".png", ".png.png");
                });

                return result;
            }catch(Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            
        }
        [Authorize]
        [HttpPost("CreateComment")]
        public IActionResult CreateComment([FromBody] BookComment bc)
        {
            _context.BookComments.Add(bc);
            var result = _context.SaveChanges();

            return result > 0
                ? Created()
                : NotFound();
        }
        [Authorize]
        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int id )
        {
            var entity = new BookComment();
            entity.Id = id; 

            _context.BookComments.Remove(entity);
            var result = _context.SaveChanges();

            return result > 0 
                ? Ok() 
                : NotFound();
        }

        [HttpGet("TestORM")]
        public object TestORM()
        {
            var result = _context.Books.ToList();
            var _service = new BookService(_context);
            var resultd = _service.GetBookWithouthThumbNail();
            var res = _context.BookFileStorages.ToList();
            var res3 = _context.FileStorage.ToList();
            var resultds =_service.GetTest();
            var d1 = new DirectoryInfo(resultds.ElementAt(0));
            var d2 = new DirectoryInfo(resultds.ElementAt(1));
            return result.First();
        }
    }
}
