using CtrlAltEliteProject.Services.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register application services
builder.Services.AddScoped<CtrlAltEliteProject.Services.IPdfTextService, CtrlAltEliteProject.Services.PdfTextService>();
builder.Services.AddSingleton<IUserQueryLogger, UserQueryLogger>();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Map controller routes and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();