using System.ComponentModel.DataAnnotations;

namespace Identity;

public class RegisterUserViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
}