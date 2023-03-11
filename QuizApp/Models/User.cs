using Microsoft.AspNetCore.Identity;

namespace QuizApp.Models;

public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public List<QuizResult> QuizResults = new List<QuizResult>();
}