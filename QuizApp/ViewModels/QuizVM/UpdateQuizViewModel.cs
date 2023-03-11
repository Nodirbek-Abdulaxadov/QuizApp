using QuizApp.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.ViewModels.QuizVM;

public class UpdateQuizViewModel : BaseModel
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [StringLength(600)]
    public string Description { get; set; } = string.Empty;
}