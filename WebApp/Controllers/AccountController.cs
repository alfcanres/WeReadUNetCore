﻿using Microsoft.AspNetCore.Mvc;
using DataTransferObjects.DTO;
using WebAPI.Client.Repository.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebAPI.Client.ViewModels;


namespace WebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;


        public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
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

            var response = await _accountRepository.LoginAsync(loginViewModel);

            if (response.Status == ResponseStatus.Success)
            {


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginViewModel.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Response.Cookies.Append("AuthToken", response.Content.Token, new CookieOptions { HttpOnly = true, Secure = true });

                return RedirectToAction("Index", "Home");
            }
            else 
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(loginViewModel);
            }
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateDTO);
            }

            var response = await _accountRepository.RegisterAsync(userCreateDTO);

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("RegisterSucess", "Account");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(userCreateDTO);
            }
        }


        public IActionResult RegisterSucess()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index", "Home");
        }
    }
}
