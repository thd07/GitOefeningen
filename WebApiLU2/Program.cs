using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using WebApiLU2.Repository;
using WebApiLU2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // ?? Laad de standaard JSON-configuratie
    .AddUserSecrets<Program>(optional: true) // ?? Laad User Secrets
    .AddEnvironmentVariables(); // ?? Laad omgevingsvariabelen

var jwtSettings = builder.Configuration.GetSection("JwtSettings");



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

var sqlConnectionStringg = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});



builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

builder.Services.AddTransient<IObject2dRepository, Object2dRepository>(o => new Object2dRepository(sqlConnectionString));


// regel commetaar

var app = builder.Build();
app.MapGet("/", () => $"The API is up Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

app.UseAuthorization();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
