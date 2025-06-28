using System.Data.Common;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using WebApiLU2.Repository;
using WebApiLU2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true) 
    .AddEnvironmentVariables(); 





//Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString2"];
if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

var sqlConnectionStringg = builder.Configuration.GetValue<string>("SqlConnectionString2");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
var sqlConnectionString2 = builder.Configuration.GetConnectionString("SqlConnectionString2");
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
   
    options.Password.RequiredLength = 10;
    //options.Password.RequireUppercase = true;
    //options.Password.RequireDigit = true;
    //options.Password.RequireNonAlphanumeric=true;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});



builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationServices, AspNetIdentityAuthenticationService>();

builder.Services.AddTransient<IObject2dRepository, Object2dRepository>(o => new Object2dRepository(sqlConnectionString));
builder.Services.AddTransient<IEnvironment2dRepository, Environment2dRepository>(o => new Environment2dRepository(sqlConnectionString));


builder.Services
    .AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
    .Configure(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromMinutes(value: 120);
    });


// regel commetaar

var app = builder.Build();


app.MapGet("/", () => $"The API is up Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");

app.UseAuthorization();
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();
