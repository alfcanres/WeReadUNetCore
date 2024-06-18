using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApp.Models;
using System.Text.Json;
using DataTransferObjects.DTO;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;


        public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserSignInDTO loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            string clientName = _configuration["HttpClientName"];   

            var client = _httpClientFactory.CreateClient(clientName);
            var content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/account/login", content);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponseViewModel>(
                    await response.Content.ReadAsStreamAsync(), 
                    new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true }
                    );

                
                if(!tokenResponse.Validate.IsValid)
                {
                    foreach(var message in tokenResponse.Validate.MessageList)
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
    
                    return View(loginViewModel);
                }
                else
                {

                    HttpContext.Response.Cookies.Append("AuthToken", tokenResponse.Token, new CookieOptions { HttpOnly = true, Secure = true });
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginViewModel);
            }

        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
