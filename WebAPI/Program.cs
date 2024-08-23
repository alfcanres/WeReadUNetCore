using BusinessLogicLayer;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
builder.Logging.AddDebug();
builder.Logging.AddConsole();


//Disable model validation
builder.Services.Configure<ApiBehaviorOptions>(op =>
{
    op.SuppressModelStateInvalidFilter = true;
});


// Add services to the container.


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

#region Business Objects

builder.Services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();

builder.Services.AddScoped<IDataAnnotationsValidator, DataAnnotationsValidatorHelper>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAccountBLL,AccountBLL>();

builder.Services.AddScoped<IMoodTypeBLL, MoodTypeBLL>();

builder.Services.AddScoped<IPostBLL, PostBLL>();

builder.Services.AddScoped<IPostCommentBLL, PostCommentBLL>();

builder.Services.AddScoped<IPostTypeBLL, PostTypeBLL>();

builder.Services.AddScoped<IPostVoteBLL, PostVoteBLL>();

builder.Services.AddScoped<IApplicationUserInfoBLL, ApplicationUserInfoBLL>();


#endregion

builder.Services.AddResponseCaching();


builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    options => options.MigrationsAssembly("DataAccessLayer")
    ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration.GetSection("jwt:Issuer").Value,
        ValidAudience = configuration.GetSection("jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("jwt:Key").Value))
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeReadU", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
        new string[]{ }
        }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


