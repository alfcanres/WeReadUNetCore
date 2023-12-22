using AutoMapper;
using BusinessLogicLayer;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

#region Business Objects

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddScoped<IPostTypeBLL, PostTypeBLL>();

#endregion

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    options => options.MigrationsAssembly("DataAccessLayer") //Se pone para que las migraciones se guarden en el proyecto DataAccessLayer que está fuera de este proyecto CambalachApi
    ));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
