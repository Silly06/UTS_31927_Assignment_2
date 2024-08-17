using PetPlayApp.Server.Db;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddDbContext<DatabaseContext>()
	.AddScoped<IRepositoryProviderService, RepositoryProviderService>()
	.AddScoped<IPostService, PostService>()
	.AddScoped<IUserService, UserService>()
	.AddScoped<IMatchService, MatchService>()
	.AddScoped<ICommentService, CommentService>()
	.AddScoped<INotificationService, NotificationService>()
	.AddScoped<ISeedService, SeedService>()
	.AddScoped<IStoryService, StoryService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
	seedService.SeedData();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
