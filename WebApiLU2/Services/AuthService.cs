using System.Threading.Tasks;
using Dapper;
using System.Data;
using WebApiLU2.Data;
using WebApiLU2.Models;
using WebApiLU2.Services;

public class AuthService
{
    private readonly DapperDbContext _db;
    private readonly JwtTokenGenerator _jwtGenerator;

    public AuthService(DapperDbContext db, JwtTokenGenerator jwtGenerator)
    {
        _db = db;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password)
    {
        using var connection = _db.CreateConnection();

        var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM AspNetUsers WHERE Username = @Username", new { Username = username });

        if (existingUser != null)
            return (false, "Gebruiker bestaat al.");

        var user = new User
        {
            Username = username,
            Password = password // 🔥 LET OP: Wachtwoord wordt in platte tekst opgeslagen!
        };

        await connection.ExecuteAsync(
            "INSERT INTO AspNetUsers (Username, Password) VALUES (@Username, @Password)", user);

        return (true, "Gebruiker succesvol geregistreerd!");
    }

    public async Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password)
    {
        using var connection = _db.CreateConnection();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM AspNetUsers WHERE Username = @Username AND Password = @Password",
            new { Username = username, Password = password });

        if (user == null)
            return (false, null, "Ongeldige gebruikersnaam of wachtwoord.");

        string token = _jwtGenerator.GenerateToken(user);
        return (true, token, "Login succesvol!");
    }
}
