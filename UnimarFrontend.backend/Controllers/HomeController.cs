using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UnimarFrontend.backend.UnimarFrontend.Dominio.Entidades;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;

namespace UnimarFrontend.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetBooks")]
        public IEnumerable<Book> GetBooks(int page)
        {
            var result = _context.Books
                .Skip(page * 5)
                .Take(5)
                .ToList();

            return result;
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

    }
}
