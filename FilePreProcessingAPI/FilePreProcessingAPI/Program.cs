/*
 * Developer: Evan S.
 * Date Created: 10/19/2025
 * Last Updated: 10/23/2025
 * Description: For CS 478 Team Project
 *              
*/

using FilePreProcessingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Minimal: controllers + pdf service (no Swagger)
builder.Services.AddControllers();
builder.Services.AddScoped<PdfProcessor>();

var app = builder.Build();

// Minimal pipeline
app.MapControllers();

app.Run();