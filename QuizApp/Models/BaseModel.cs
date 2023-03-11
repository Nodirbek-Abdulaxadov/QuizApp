using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models;

public class BaseModel
{
    [Key, Required]
    public int Id { get; set; }
}
