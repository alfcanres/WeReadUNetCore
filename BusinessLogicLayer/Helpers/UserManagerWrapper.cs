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
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagerWrapper(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser appUser, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(appUser, oldPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser appUser, string password)
        {
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser appUser, string password)
        {
            return await _userManager.CreateAsync(appUser, password);
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser appUser)
        {
            return await _userManager.DeleteAsync(appUser);
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityUser> FindByIdAsync(string userID)
        {
            return await _userManager.FindByEmailAsync(userID);
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByEmailAsync(userName);
        }
    }
}
