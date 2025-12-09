using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UnimarFrontend.backend.Context;
using UnimarFrontend.backend.DTO;
using UnimarFrontend.backend.ValueObjects;
using UnimarFrontend.backend.Service;

namespace UnimarFrontend.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;
        private readonly JwtSettingsDTO _jwtSettings;
        public UserController(ILogger<UserController> logger, AppDbContext context, JwtSettingsDTO jwtSettings )
        {
            _logger = logger;
            _context = context;
            _jwtSettings = jwtSettings;
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
        [HttpPost(Name = "Authentication")]
        public ActionResult Authentication(AuthenticationDTO.Request request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email.Value == request.Email);
            var password = new PasswordVO(request.Password);

            if (user == null || !(user.PasswordHash.Hash ==  password.Hash))
            {
                return Unauthorized();
            }
            var jwtService = new JwtService(_jwtSettings);

            return Ok();
        }
    }
}
