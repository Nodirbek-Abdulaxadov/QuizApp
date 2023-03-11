using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.ViewModels.OptionVM;
using QuizApp.ViewModels.QuestionVM;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public QuestionController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _dbContext.Questions
                                       .Include(i => i.Options)
                                       .ToListAsync();

            List<QuestionViewModel> result = new List<QuestionViewModel>();

            foreach (var item in list)
            {
                result.Add(new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    QuizId = item.QuizId,
                    Options = item.Options.Select(i => new OptionViewModel()
                    {
                        Id = i.Id,
                        Text = i.Text,
                        IsCorrect = i.IsCorrect,
                        QuestionId = i.QuestionId
                    }).ToList()
                });
            }

            var json = JsonConvert.SerializeObject(result, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var quiz = await _dbContext.Questions
                                       .Include(i => i.Options)
                                       .FirstOrDefaultAsync(x => x.Id == id);

            QuestionViewModel viewModel = new()
            {
                Id = quiz.Id,
                Text = quiz.Text,
                QuizId = quiz.QuizId,
                Options = quiz.Options.Select(i => new OptionViewModel()
                {
                    Id = i.Id,
                    Text = i.Text,
                    IsCorrect = i.IsCorrect,
                    QuestionId = i.QuestionId
                }).ToList()
            };

            if (quiz == null)
            {
                return NotFound();
            }

            var json = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpPost]
        public IActionResult Post(AddQuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _dbContext.Questions.Add(new Question()
                {
                    Text = viewModel.Text,
                    QuizId = viewModel.QuizId,
                    Quiz = null
                });

                _dbContext.SaveChanges();

                return Ok(model.Entity);
            }

            return BadRequest();
        }

        [HttpPut]
        public IActionResult Put(UpdateQuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _dbContext.Questions.Update(new Question()
                {
                    Id = viewModel.Id,
                    Text = viewModel.Text,
                    QuizId = viewModel.QuizId,
                    Quiz = null
                });

                _dbContext.SaveChanges();

                return Ok(model.Entity);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _dbContext.Questions.FirstOrDefaultAsync(i => i.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            _dbContext.Questions.Remove(model);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
