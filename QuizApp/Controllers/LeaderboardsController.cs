using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class LeaderboardsController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public LeaderboardsController(AppDbContext dbContext,
                                  UserManager<User> userManager)
	{
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet("{quizId}")]
    public async Task<IActionResult> Get(int quizId)
    {
        var results = await _dbContext.QuizResults.ToListAsync();
        List<LeaderViewModel> leaders = new List<LeaderViewModel>();
        foreach (var result in results)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == result.UserId);
            if (user == null)
            {
                continue;
            }
            if (!leaders.Any(u => u.Email == user.Email))
            {
                leaders.Add(new LeaderViewModel()
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Score = result.Score
                }) ;
            }

            foreach (var leader in leaders)
            {
                leader.Score = result.Score;
                leader.Email = user.Email;
                leader.FullName = user.FullName;
            }
        }

        return Ok(leaders);
    }
}