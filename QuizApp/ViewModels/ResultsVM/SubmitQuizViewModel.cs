using System.ComponentModel.DataAnnotations;

namespace QuizApp.ViewModels.ResultsVM;

public class SubmitQuizViewModel
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public int QuizId { get; set; }
}