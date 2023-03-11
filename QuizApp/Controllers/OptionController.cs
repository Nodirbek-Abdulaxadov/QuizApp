using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.ViewModels.OptionVM;

namespace QuizApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OptionController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public OptionController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var list = await _dbContext.Options.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var quiz = await _dbContext.Options.FirstOrDefaultAsync(x => x.Id == id);
        if (quiz == null)
        {
            return NotFound();
        }
        return Ok(quiz);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post(AddOptionViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var model = _dbContext.Options.Add(new Option()
            {
                Text = viewModel.Text,
                IsCorrect = viewModel.IsCorrect,
                QuestionId = viewModel.QuestionId,
                Question = null
            });

            await _dbContext.SaveChangesAsync();

            return Ok(model.Entity);
        }

        return BadRequest();
    }

    [HttpPut]
    [AllowAnonymous]
    public IActionResult Put(UpdateOptionViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var model = _dbContext.Options.Update(new Option()
            {
                Id = viewModel.Id,
                Text = viewModel.Text,
                IsCorrect = viewModel.IsCorrect,
                QuestionId = viewModel.QuestionId,
                Question = null
            });

            _dbContext.SaveChanges();

            return Ok(model.Entity);
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _dbContext.Options.FirstOrDefaultAsync(i => i.Id == id);
        if (model == null)
        {
            return NotFound();
        }

        _dbContext.Options.Remove(model);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
