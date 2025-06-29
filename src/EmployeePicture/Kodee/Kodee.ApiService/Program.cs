using Kodee.ApiService.Models;
using Kodee.ApiService.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.

// TimeProvider를 서비스로 추가
//builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddProblemDetails();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EmployeePhotoDbContext>(options => 
    options.UseSqlServer(connectionString));

// 인증 스키마 추가
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/{name}.json 엔드포인트 등록
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "weather api");
        //options.RoutePrefix = string.Empty; // 루트에서 UI 노출
        //options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();

//string email = "admin@visualacademy.com";
//string password = "securepassword";
//string credentials = $"{email}:{password}";
//string base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
//Console.WriteLine($"Authorization: Basic {base64Credentials}");
