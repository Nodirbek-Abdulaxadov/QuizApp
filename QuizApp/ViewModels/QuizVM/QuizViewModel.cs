using QuizApp.Models;
using QuizApp.ViewModels.QuestionVM;

namespace QuizApp.ViewModels.QuizVM;

public class QuizViewModel : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<QuestionViewModel> Questions = new List<QuestionViewModel>();
}