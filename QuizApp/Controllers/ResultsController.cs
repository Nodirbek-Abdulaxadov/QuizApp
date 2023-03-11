using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.ViewModels.ResultsVM;

namespace QuizApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ResultsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ResultsController(AppDbContext dbContext)
	{
        _dbContext = dbContext;
    }

    [HttpGet("all/{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var results = await _dbContext.QuizResults.ToListAsync();
        results = results.Where(r => r.UserId == userId).ToList();
        return Ok(results);
    }

    [HttpGet]
    public async Task<IActionResult> Get(string userId, int quizId)
    {
        var list = await _dbContext.QuizResults.ToListAsync();
        var results = list.Where(r => r.UserId == userId && r.QuizId == quizId)
                         .ToList();

        return Ok(results);
    }

    [HttpPost]
    public IActionResult Submit([FromQuery]SubmitQuizViewModel viewModel, [FromBody] List<Answer> Answers)
    {
        var questions = _dbContext.Questions.Where(q => q.QuizId == viewModel.QuizId)
                                                  .ToList();

        int score = 0;
        foreach (Answer answer in Answers)
        {
            var options = _dbContext.Options.Where(o => o.QuestionId == answer.QuestionId)
                                                  .ToList();
            var option = options.FirstOrDefault(o => o.Id == answer.OptionId);
            if (option?.IsCorrect == true)
            {
                score++;
            }
        }

        QuizResult result = new()
        {
            QuizId = viewModel.QuizId,
            UserId = viewModel.UserId,
            Score = score,
            User = null,
            Quiz = null
        };

        _dbContext.QuizResults.Add(result);
        _dbContext.SaveChanges();

        return Ok(result);
    }
}