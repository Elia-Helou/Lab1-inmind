using Lab1.API.Dtos;
using Lab1.Domain.Models;
using Lab1.Domain.Services;
using Lab1.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Lab1.API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;
        private readonly UserValidator _validator;

        public UserController(IUserService userService, UserValidator userValidator) 
        {
            _users = userService;
            _validator = userValidator;
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUsers()
        {
            var users = _users.GetAllUsers();
            var persons = users.Select(user => ObjectMapperService.Map<User, Person>(user)).ToList();
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 0)
                throw new ArgumentException("ID cannot be less than 0.");

            var user = _users.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            var person = ObjectMapperService.Map<User, Person>(user);

            return Ok(person);
        }

        [HttpGet("search")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name can't be null or empty");
            }
            var allUsers = _users.GetUsersByName(name);

            if (allUsers == null || !allUsers.Any())
                throw new KeyNotFoundException($"No users found with the name {name}.");

            var persons = allUsers.Select(user => ObjectMapperService.Map<User, Person>(user)).ToList();

            return Ok(persons);
        }
        

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be less than 0.");

            var result = _users.DeleteUser(id);
            if (!result)
            {
                throw new KeyNotFoundException($"No users found with the id {id}.");
            }
            return Ok();
        }

        [HttpPost("update")]
        public IActionResult UpdateUser(User user)
        {
            var validationResult = _validator.Validate(user);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage })
                    .ToList();

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }

            var result = _users.UpdateUser(user);
            if (!result)
            {
                throw new KeyNotFoundException($"User with id {user.Id} not found");
            }
            return Ok(new { Message = "User Updated Successfully" });
        }

    }
}
