using QuizApp.Models;

namespace Identity;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 

    public static explicit operator UserDto(User v)
        => new UserDto()
        {
            Id = v.Id,
            FullName = v.FullName??"",
            Email = v.Email??""
        };
}