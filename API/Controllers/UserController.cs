using Lab1.Domain.Models;
using Lab1.Domain.Services;
using Lab1.Domain.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;
        private readonly IUserValidator _validator;

        public UserController(IUserService userService, IUserValidator userValidator) 
        {
            _users = userService;
            _validator = userValidator;
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_users.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred", Details = ex.Message });
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult Get(int id)
        {
            var (isValid, errorMessage) = _validator.ValidateUserId(id);
            if (!isValid)
                return BadRequest(errorMessage);

            var user = _users.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("search")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            var (isValid, errorMessage) = _validator.ValidateName(name);

            if(!isValid)
                return BadRequest(errorMessage);

            var allUsers = _users.GetUsersByName(name);

            if (allUsers == null || !allUsers.Any())
                return NotFound($"No users found with the name {name}.");

            return Ok(allUsers);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            var (isValid, errorMessage) = _validator.ValidateUserId(id);
            if (!isValid)
                return BadRequest(errorMessage);

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
            var (isNameValid, nameErrorMessage) = _validator.ValidateName(user.Name);
            if (!isNameValid)
                return BadRequest(nameErrorMessage);

            var (isEmailValid, emailErrorMessage) = _validator.ValidateEmail(user.Email);
            if (!isEmailValid)
                return BadRequest(emailErrorMessage);

            var result = _users.UpdateUser(user);
            if (!result)
            {
                return NotFound(new {Message = $"user with id {user.Id} not found"});
            }
            return Ok(new {Message = "User Updated Successfully"});
        }

    }
}
