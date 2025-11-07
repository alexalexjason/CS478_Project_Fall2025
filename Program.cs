using Microsoft.Extensions.Options;
using WebApplication1.Services;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// configure Hugging Face options from configuration (appsettings or secrets)
builder.Services.Configure<HuggingFaceOptions>(builder.Configuration.GetSection("HuggingFace"));

// Register Hugging Face service using HttpClientFactory
builder.Services.AddHttpClient<IHuggingFaceService, HuggingFaceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();