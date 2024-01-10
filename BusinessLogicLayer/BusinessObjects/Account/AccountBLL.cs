using AutoMapper;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessObjects
{
    public class AccountBLL
    {
        protected readonly IValidate _validate = new ValidateDTO();
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountBLL(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<UserReadDTO> InsertAsync(UserCreateDTO userCreateDTO)
        {
            var user = new ApplicationUser { UserName = userCreateDTO.UserName, Email = userCreateDTO.Email };
            var result = await _userManager.CreateAsync(user, userCreateDTO.Password);
            if (result.Succeeded)
            {
                return new UserReadDTO
                {
                    Email = userCreateDTO.Email,
                    UserName = userCreateDTO.UserName
                };
            }
            else
            {
                //result.Errors
                return null;

            }
        }

        public async Task<UserReadDTO> SigInAsync(UserSignInDTO userSignInDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(userSignInDTO.Email, userSignInDTO.Password, isPersistent: false, lockoutOnFailure: false);
            var user = await _userManager.FindByEmailAsync(userSignInDTO.Email);

            if (result.Succeeded)
            {
                return new UserReadDTO
                {
                    Email = userSignInDTO.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = $"{user.FirstName} {user.LastName}"
                };
            }
            else
            {
                //result.Errors
                return null;

            }
        }
    }
}
