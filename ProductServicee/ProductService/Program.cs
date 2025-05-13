using Microsoft.Extensions.DependencyInjection;
using ProductService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureProductRepository();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsProduction())
    app.UseHsts();


app.MapControllers();

app.Run();
