namespace QuizApp.Models;

public class Option : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public Question Question = new Question();
}