using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.ViewModels.OptionVM;
using QuizApp.ViewModels.QuestionVM;
using QuizApp.ViewModels.QuizVM;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public QuizController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _dbContext.Quizzes.ToListAsync();
            return Ok(list);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetWithQuestions()
        {
            var list = await _dbContext.Quizzes.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var quiz = await _dbContext.Quizzes
                                       .Include(i => i.Questions)
                                       .ThenInclude(j => j.Options)
                                       .FirstOrDefaultAsync(x => x.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }
            QuizViewModel vm = new()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.Questions.Select(i => new QuestionViewModel()
                {
                    Id = i.Id,
                    Text = i.Text,
                    QuizId = i.QuizId,
                    Options = i.Options.Select(o => new OptionViewModel()
                    {
                        Id = o.Id,
                        Text = o.Text,
                        IsCorrect = o.IsCorrect,
                        QuestionId = o.QuestionId
                    }).ToList()
                }).ToList()
            };

            var json = JsonConvert.SerializeObject(vm, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpPost]
        public IActionResult Post(AddQuizViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _dbContext.Quizzes.Add(new Quiz()
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description
                });

                _dbContext.SaveChanges();

                return Ok(model.Entity);
            }

            return BadRequest();
        }

        [HttpPut]
        public IActionResult Put(UpdateQuizViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _dbContext.Quizzes.Update(new Quiz()
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Description = viewModel.Description
                });

                _dbContext.SaveChanges();

                return Ok(model.Entity);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = _dbContext.Quizzes.FirstOrDefault(i => i.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            _dbContext.Quizzes.Remove(model);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
