namespace Lab1.Domain.Validators
{
    public interface IUserValidator
    {
        (bool IsValid, string ErrorMessage) ValidateUserId(long id);
        (bool IsValid, string ErrorMessage) ValidateName(string name);
        (bool IsValid, string ErrorMessage) ValidateEmail(string email);
    }
}
