using Lab1.Domain.Services;
using Lab1.Domain.Validators;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class UserValidator : IUserValidator
{
    private readonly IUserService _userService;

    public UserValidator(IUserService userService)
    {
        _userService = userService;
    }

    public (bool IsValid, string ErrorMessage) ValidateUserId(long id)
    {
        if (id <= 0)
            return (false, "Invalid user ID. ID must be a positive number.");

        if (_userService.GetById(id) == null)
            return (false, $"User with ID {id} does not exist.");

        return (true, string.Empty);
    }
    public (bool IsValid, string ErrorMessage) ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "name cannot be empty or white space.");

        return (true, string.Empty);
    }

    public (bool IsValid, string ErrorMessage) ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email cannot be empty.");

        if (!new EmailAddressAttribute().IsValid(email))
            return (false, "Invalid email address.");

        return (true, string.Empty);
    }
}
