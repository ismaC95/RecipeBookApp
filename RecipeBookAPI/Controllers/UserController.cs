using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBookAPI.Models;
using RecipeBookAPI.Services;

namespace RecipeBookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserServices _userService;

        public UserController(ILogger<UserController> logger, UserServices userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public User? Get(int id)
        {
            //if not dependency injection then we can use:
            //var userService = new UserService();
            User? user = _userService.GetUser(id);
            return user;
        }

        [HttpPost("Register")]
        public void RegisterUser(string name, string email, string password)
        {
            _userService.RegisterUser(name, email, password);
        }
    }
}
