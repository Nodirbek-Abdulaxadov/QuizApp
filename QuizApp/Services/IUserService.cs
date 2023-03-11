using Identity;

namespace QuizApp.Services;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(RegisterUserViewModel viewModel);
    Task<string> LoginUserAsync(LoginUserViewModel viewModel);
    Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequstViewModel viewModel);
    Task Logout(string refreshToken);
}