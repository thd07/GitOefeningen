using WebApiLU2.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

var sqlConnectionStringg = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddTransient<IObject2dRepository, Object2dRepository>(o => new Object2dRepository(sqlConnectionString));


// regel commetaar

var app = builder.Build();
app.MapGet("/", () => $"The API is up Connection string found: {(sqlConnectionStringFound ? "yes" : "no")}");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
