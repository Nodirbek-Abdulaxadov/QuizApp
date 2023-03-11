using QuizApp.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.ViewModels.QuestionVM;

public class UpdateQuestionViewModel : BaseModel
{
    [Required]
    [StringLength(1000)]
    public string Text { get; set; } = string.Empty;
    [Required]
    public int QuizId { get; set; }
}