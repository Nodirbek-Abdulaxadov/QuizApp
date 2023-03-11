namespace QuizApp.Models;

public class Quiz : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<Question> Questions = new List<Question>();
}