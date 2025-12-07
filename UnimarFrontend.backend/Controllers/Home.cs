using Microsoft.AspNetCore.Mvc;
using UnimarFrontend.backend.Context;
using UnimarFrontend.backend.Models;

namespace UnimarFrontend.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        private readonly ILogger<Home> _logger;
        private readonly AppDbContext _context;

        public Home(ILogger<Home> logger , AppDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetBooks")]
        public IEnumerable<Book> Get()
        {
            var result = _context.Books.ToList();
            return result;
        }
    }
}
