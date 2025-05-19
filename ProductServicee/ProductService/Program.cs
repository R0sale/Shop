using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.ActionFilters;
using ProductService.Extensions;
using Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureProductRepository();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureServiceManager();

builder.Services.ConfigureValidationFilter();
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<ProductRepositoryContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.ConfigureSqlContext(builder.Configuration);
}

builder.Services.ConfigureHttpClient();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

app.ConfigureExceptionHandler();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
