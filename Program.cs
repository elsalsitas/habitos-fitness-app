using MongoDBReports.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configurar Reports como página por defecto
app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Reports"));

app.Run();
