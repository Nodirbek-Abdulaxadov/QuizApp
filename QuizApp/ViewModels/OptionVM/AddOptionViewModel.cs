using System.ComponentModel.DataAnnotations;

namespace QuizApp.ViewModels.OptionVM;

public class AddOptionViewModel
{
    [Required]
    [StringLength(200)]
    public string Text { get; set; } = string.Empty;
    [Required]
    public bool IsCorrect { get; set; }
    [Required]
    public int QuestionId { get; set; }
}