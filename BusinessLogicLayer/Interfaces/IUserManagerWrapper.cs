using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserManagerWrapper
    {
        Task<ApplicationUser> FindByEmailAsync(string email);

        Task<ApplicationUser> FindByNameAsync(string userName);

        Task<ApplicationUser> FindByIdAsync(string userName);

        Task<IdentityResult> CreateAsync(ApplicationUser appUser, string password);

        Task<bool> CheckPasswordAsync(ApplicationUser appUser, string password);

        Task<IdentityResult> DeleteAsync(ApplicationUser appUser);

        Task<IdentityResult> ChangePasswordAsync(ApplicationUser appUser, string oldPassword, string newPassword);
    }
}
