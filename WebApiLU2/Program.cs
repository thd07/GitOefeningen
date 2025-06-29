using System.Data.Common;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using WebApiLU2.Repository;
using WebApiLU2.Services;



using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ADD SWAGGER GENERATION SERVICES (NEW)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiLU2", Version = "v1" });

    // Optional: If you use authorization, configure security scheme here
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var sqlConnectionString = builder.Configuration.GetConnectionString("SqlConnectionString");
if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

var sqlConnectionStringg = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
var sqlConnectionString2 = builder.Configuration.GetConnectionString("SqlConnectionString");
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
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

// ADD SWAGGER MIDDLEWARE (NEW)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiLU2 V1");
        c.RoutePrefix = "swagger"; // You can change this or set to "" for root
    });

    app.MapOpenApi();  // you can keep your existing OpenAPI mapping
}

app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
