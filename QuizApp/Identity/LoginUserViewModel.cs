using System.ComponentModel.DataAnnotations;

namespace Identity;

public class LoginUserViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}