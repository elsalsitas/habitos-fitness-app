using MongoDBReports.Services;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Configuración mínima para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

=======
// Configurar para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
builder.Services.AddRazorPages();
builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

<<<<<<< HEAD
=======
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
