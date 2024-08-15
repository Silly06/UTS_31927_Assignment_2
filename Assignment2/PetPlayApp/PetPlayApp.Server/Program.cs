using PetPlayApp.Server.Db;
using PetPlayApp.Server.Db.Repos;
using PetPlayApp.Server.Db.Services;
using PetPlayApp.Server.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContext<DatabaseContext>()
    .AddScoped<RepositoryProvider>()
    .AddScoped<UserService>()
    .AddScoped<MatchService>()
    .AddScoped<SeedService>();

builder.Services.AddControllers();

var app = builder.Build();

app.Services.GetRequiredService<SeedService>().SeedData();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
