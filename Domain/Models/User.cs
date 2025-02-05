using System.ComponentModel.DataAnnotations;

namespace Lab1.Domain.Models
{
    public class User
    {
        [Required(ErrorMessage = "User Id is required")]
        [Range (1, long.MaxValue, ErrorMessage = "Id must be a positive number.")]
        public long Id { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "User Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
    }
}
