namespace QuizApp.Models;

public class QuizResult : BaseModel
{
    public string UserId { get; set; } = string.Empty;
    public User User = new User();
    public int QuizId { get; set; }
    public Quiz Quiz = new Quiz();
    public double Score { get; set; }
}