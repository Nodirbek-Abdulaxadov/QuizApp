using QuizApp.Models;
using QuizApp.ViewModels.OptionVM;

namespace QuizApp.ViewModels.QuestionVM;

public class QuestionViewModel : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public List<OptionViewModel> Options = new List<OptionViewModel>();
}