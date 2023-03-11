using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Reflection.Emit;

namespace QuizApp.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question>()
                .HasOne(s => s.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(i => i.QuizId);

            builder.Entity<Option>()
                .HasOne(s => s.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(i => i.QuestionId);

            builder.Entity<QuizResult>()
                .HasOne(i => i.User)
                .WithMany(q => q.QuizResults)
                .HasForeignKey(j => j.UserId);


            base.OnModelCreating(builder);
        }
    }
}
