using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Client.Repository.Account;
using WebAPI.Client.Repository.MoodType;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMoodTypeRepository,MoodTypeRepository>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient(builder.Configuration["HttpClientName"], client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
}
);


// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to the login page
        options.LogoutPath = "/Account/Logout"; // Path to the logout page
        options.AccessDeniedPath = "/Account/Login"; // Path to the access denied page
        options.ExpireTimeSpan = TimeSpan.FromDays(1); // Cookie expiration time
        options.SlidingExpiration = true; // Enable sliding expiration
    });


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
