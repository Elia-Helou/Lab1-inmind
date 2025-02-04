using Lab1.Domain.Models;
using Lab1.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly UserService _users;
        public UserController(UserService userService) 
        {
            _users = userService;
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUsers()
        {
            return Ok(_users.GetAllUsers());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult Get(int id)
        { 
            var user = _users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("search")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            return Ok(_users.GetUsersByName(name));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            var result = _users.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("update")]
        public IActionResult UpdateUser(User user)
        {
            var result = _users.UpdateUser(user);
            if (!result)
            {
                return NotFound(new {Message = $"user with id {user.Id} not found"});
            }
            return Ok(new {Message = "User Updated Successfully"});
        }

    }
}
