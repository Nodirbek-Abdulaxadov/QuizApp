using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QuizApp.Data;
using QuizApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizApp.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly TokenValidationParameters _validationParameters;

    public UserService(UserManager<User> userManager,
                       IConfiguration configuration,
                       AppDbContext dbContext,
                       TokenValidationParameters validationParameters)
    {
        _userManager = userManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _validationParameters = validationParameters;
    }
    public async Task<UserDto> CreateUserAsync(RegisterUserViewModel viewModel)
    {
        var userExist = await _userManager.FindByEmailAsync(viewModel.Email);
        if (userExist != null)
        {
            throw new Exception("This email is already exist!");
        }

        User user = new()
        {
            FullName = viewModel.FullName,
            Email = viewModel.Email,
            UserName = viewModel.FullName.Replace(" ", ""),
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        if (result.Succeeded)
        {
            return (UserDto)user;
        }

        throw new Exception(result.Errors.First().Description);
    }

    public async Task<string> LoginUserAsync(LoginUserViewModel viewModel)
    {
        var userExist = await _userManager.FindByEmailAsync(viewModel.Email);

        var passwordIsValid = await _userManager.CheckPasswordAsync(userExist, viewModel.Password);

        if (userExist != null && passwordIsValid)
        {
            var token = _dbContext.RefreshTokens.FirstOrDefault(r => r.UserId == Guid.Parse(userExist.Id));
            if (token != null)
            {
                _dbContext.RefreshTokens.Remove(token);
                _dbContext.SaveChanges();
            }

            return JsonConvert.SerializeObject(await GenerateTokenAsync(userExist, null));
        }

        throw new Exception("Login failed! Incorrect email or password!");
    }

    private async Task<AuthResultViewModel> GenerateTokenAsync(User user, RefreshToken refresh)
    {
        var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FullName??""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var authKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:securityKey"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audence"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256));

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (refresh != null)
        {
            var rToken = new AuthResultViewModel()
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = jwtToken,
                RefreshToken = refresh.Token,
                ExpiresAt = token.ValidTo,
            };
            return rToken;
        }

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = Guid.Parse(user.Id),
            DateAdded = DateTime.UtcNow.ToString(),
            DataExpire = DateTime.UtcNow.AddMonths(1).ToString(),
            Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        var response = new AuthResultViewModel()
        {
            FullName = user.FullName,
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = token.ValidTo,
        };

        return response;
    }

    public async Task<AuthResultViewModel> VerifyAndGenerateTokenAsync(TokenRequstViewModel viewModel)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var storedToken = _dbContext.RefreshTokens.FirstOrDefault(i => i.Token == viewModel.RefreshToken);

        if (storedToken == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

        try
        {
            var tokenCheckResult = tokenHandler.ValidateToken(viewModel.Token,
                                                              _validationParameters,
                                                              out var validatedToken);
            return await GenerateTokenAsync(user, storedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            if (DateTime.Parse(storedToken.DataExpire) >= DateTime.UtcNow)
            {
                return await GenerateTokenAsync(user, storedToken);
            }
            else
            {
                return await GenerateTokenAsync(user, null);
            }
        }
    }

    public async Task Logout(string refreshToken)
    {
        var storedToken = _dbContext.RefreshTokens.FirstOrDefault(i => i.Token == refreshToken);

        if (storedToken == null)
        {
            throw new ArgumentNullException(nameof(storedToken));
        }

        _dbContext.RefreshTokens.Remove(storedToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAll()
        => await _userManager.Users.ToListAsync();
}