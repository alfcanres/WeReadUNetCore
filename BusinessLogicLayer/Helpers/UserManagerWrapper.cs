using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helpers
{
    public class UserManagerWrapper : IUserManagerWrapper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerWrapper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser appUser, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(appUser, oldPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser appUser, string password)
        {
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser appUser, string password)
        {
            return await _userManager.CreateAsync(appUser, password);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser appUser)
        {
            return await _userManager.DeleteAsync(appUser);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userID)
        {
            return await _userManager.FindByEmailAsync(userID);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByEmailAsync(userName);
        }
    }
}
