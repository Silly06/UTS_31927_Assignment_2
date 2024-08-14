using PetPlayApp.Server.Db;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Db.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContext<DatabaseContext>()
    .AddScoped<UserRepository>()
    .AddScoped<MatchRepository>()
    .AddScoped<PostRepository>()
    .AddScoped<UserService>()
    .AddScoped<MatchService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
