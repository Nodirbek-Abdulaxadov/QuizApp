using Identity;
using QuizApp.Models;

namespace QuizApp.Services;

public interface IUserService
{
    Task<List<User>> GetAll();
    Task<UserDto> CreateUserAsync(RegisterUserViewModel viewModel);
    Task<string> LoginUserAsync(LoginUserViewModel viewModel);
    Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequstViewModel viewModel);
    Task Logout(string refreshToken);
}