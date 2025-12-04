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
            User? user = _userService.GetUser(id);
            return user;
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser(string name, string email, string password)
        {
            try
            {
                _userService.RegisterUser(name, email, password);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(User updatedUser, int userID)
        {
            try
            {
                _userService.UpdateUser(updatedUser, userID);
                return Ok("User information has been updated");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                _userService.Login(email, password);
                return Ok("Welcome back");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
