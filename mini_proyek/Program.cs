using mini_proyek.Interfaces;
using mini_proyek.Models;
using mini_proyek.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DbContex>(sql => sql.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<KategoriInterfaces, KategoriServices>();
builder.Services.AddScoped<AreaInterfaces, AreaServices>();
builder.Services.AddScoped<SlotInterface, SlotsServices>();
builder.Services.AddScoped<HistoryInterface, HistoryServices>();

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
