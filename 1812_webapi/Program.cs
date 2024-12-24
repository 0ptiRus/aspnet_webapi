using _1812_webapi;
using _1812_webapi.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilder, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(hostBuilder.Configuration));
builder.Logging.AddConsole();

builder.Services.AddScoped<MyController>();
builder.Services.AddLogging();
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.

//app.MapGet("/product/all", (MyController controller) => controller.GetAll());
//app.MapGet("/product/", (MyController controller) => controller.GetAll());
//app.MapGet("/", (MyController controller) => controller.GetAll());


//app.MapGet("/product/{id:int}", (MyController controller, int id) => controller.GetSingle(id));

//app.MapPost("/product/", (MyController controller, [FromBody] Product product) => controller.Add(product));

//app.MapPut("/product/", (MyController controller, [FromBody] Product product) => controller.Update(product));

//app.MapDelete("/product/{id:int}", (MyController controller, int id) => controller.Delete(id));


app.Run();

