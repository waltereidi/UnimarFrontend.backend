using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UnimarFrontend.backend.Context;
using UnimarFrontend.backend.DTO;

namespace UnimarFrontend.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost(Name = "CreateUser")]
        public ActionResult CreateUser(CreateUserDTO.Request request)
        {
            var entity = request.ToUserEntity();
            
            _context.Users.Add(entity);
            var result = _context.SaveChanges(); 

            return result == 1 
                ? Created() 
                : UnprocessableEntity();
        }
    }
}
