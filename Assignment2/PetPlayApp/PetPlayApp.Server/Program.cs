using PetPlayApp.Server.Db;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContext<DatabaseContext>()
    .AddScoped<RepositoryProvider>()
	.AddScoped<IPostService, PostService>()
	.AddScoped<UserService>()
    .AddScoped<MatchService>()
	.AddScoped<CommentService>()
	.AddScoped<NotificationService>()
    .AddScoped<SeedService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
	seedService.SeedData();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
