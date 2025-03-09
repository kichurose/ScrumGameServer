using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ScrumGame;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Allow Angular app
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); //Allow sending authentication headers
    });
});

// updated launch settings, program.cs
// 127.0.0.1 scrumgame host file C:\Windows\System32\drivers\etc\hosts
// ping scrumgame
//dotnet dev-certs https --trust
builder.WebHost.UseUrls("https://scrumgame", "http://scrumgame");

// ✅ Add Authentication & JWT Configuration
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// ✅ Enable Authorization
builder.Services.AddAuthorization();
var app = builder.Build();


app.UseCors("AllowAngular"); // allow angular     . 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// ✅ Enable Authentication & Authorization Middleware
app.UseAuthentication(); // 🔥 This is REQUIRED before UseAuthorization()
app.UseAuthorization();




app.MapControllers();

app.Run();

// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//  error -updatedteh target framerwork with.net9from .net 8
// "Jwt": {
//"Key": "YourSuperSecretKey123456789",
//  "Issuer": "YourIssuer"
//}

