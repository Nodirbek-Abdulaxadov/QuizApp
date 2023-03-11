using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models;
public class Question : BaseModel 
{
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public Quiz Quiz = new Quiz();
    public List<Option> Options = new List<Option>();
}