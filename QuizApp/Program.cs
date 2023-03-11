using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string CORSOpenPolicy = "OpenCORSPolicy";

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add dbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LocalDB")));

//Add Identity
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddTransient<IUserService, UserService>();

//Add Authentication
var tokenParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:securityKey"] ?? "")),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

    ValidateAudience = true,
    ValidAudience = builder.Configuration["JwtSettings:Audence"],
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenParameters);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenParameters;
    });

//Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
      });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(CORSOpenPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();