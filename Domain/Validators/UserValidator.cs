using FluentValidation;
using Lab1.Domain.Models;
using Lab1.Domain.Services;

namespace Lab1.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IUserService userService)
        {
            RuleFor(user => user.Id)
                .NotEmpty()
                .WithMessage("User Id is required")
                .GreaterThan(0)
                .WithMessage("User Id must be a positive number")
                .Must(id => userService.GetById(id) != null)
                .WithMessage(id => $"User with id {id} does not exist");

            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("User Name cannot be empty or whitespace.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("User Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");
        }
    }
}