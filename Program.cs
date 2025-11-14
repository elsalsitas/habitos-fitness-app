using MongoDBReports.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddRazorPages();

// USAR SERVICIO SIMULADO TEMPORALMENTE (comenta/descomenta para cambiar)
builder.Services.AddSingleton<MongoDBServiceSimulado>();
// builder.Services.AddSingleton<MongoDBService>();

// Mejor logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
app.MapRazorPages();

app.Run();
