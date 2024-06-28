using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//inject our DbContext class and provided ConnectionString to DbContext
builder.Services.AddDbContext<NZWalksDbContext>(options =>
{ 
options.UseSqlServer("Data Source=localhost;Initial Catalog=NZWalksDb;Integrated Security=True;TrustServerCertificate=True;");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
