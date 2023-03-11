using System.ComponentModel.DataAnnotations;

namespace Identity;

public class TokenRequstViewModel
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}