using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using softsolutions.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using softsolutions.Models;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Specify the URL and port
    Urls = "http://localhost:5000"
});

byte[] secretBytes = new byte[64];
using (var random = RandomNumberGenerator.Create())
{
    random.GetBytes(secretBytes);
}

var secretKey = Convert.ToBase64String(secretBytes);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "FreeTrained",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173",
        builder => builder.WithOrigins("http://localhost:5173", "http://localhost:5173/")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddIdentityApiEndpoints<IdentityUser>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.User.RequireUniqueEmail = true;
    option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserManager<UserManager<IdentityUser>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove the options parameter from the MapIdentityApi method call
app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost5173");

app.UseAuthorization();

app.MapControllers();

app.MapPost("/create-user", async (UserManager<IdentityUser> userManager, CreateUserRequest request) =>
{
    if (request == null || string.IsNullOrWhiteSpace(request.Username) || 
        string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest("Invalid user data provided");
    }

    var user = new IdentityUser { UserName = request.Username, Email = request.Email };
    var result = await userManager.CreateAsync(user, request.Password);

    if (result.Succeeded)
    {
        return Results.Ok("User created successfully");
    }

    return Results.BadRequest(result.Errors);
});


app.Run();
