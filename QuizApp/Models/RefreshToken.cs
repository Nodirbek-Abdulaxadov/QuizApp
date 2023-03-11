namespace QuizApp.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public string DateAdded { get; set; } = string.Empty;
    public string DataExpire { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}
