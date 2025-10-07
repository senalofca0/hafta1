using Microsoft.EntityFrameworkCore;
using FabrikaAPI.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext'i servislere ekle
builder.Services.AddDbContext<FabrikaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS politikasını ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Fabrika API",
        Description = "Fabrika ürün yönetim sistemi için REST API",
        Contact = new OpenApiContact
        {
            Name = "Fabrika API Support",
            Email = "support@fabrika.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fabrika API V1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Fabrika API Documentation";
        options.DefaultModelsExpandDepth(2);
    });
}

app.UseHttpsRedirection();

// CORS middleware'ini ekle
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
