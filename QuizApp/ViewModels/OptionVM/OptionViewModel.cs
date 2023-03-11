using QuizApp.Models;

namespace QuizApp.ViewModels.OptionVM;

public class OptionViewModel : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}