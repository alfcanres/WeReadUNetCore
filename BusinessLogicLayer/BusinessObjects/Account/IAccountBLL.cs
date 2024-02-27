using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IAccountBLL
    {
        Task<IResponseDTO<UserReadDTO>> InsertAsync(UserCreateDTO createDTO);
        Task<IResponseDTO<UserReadDTO>> GetByUserNameOrEmail(string userNameOrEmail);
        public Task<IResponseDTO<UserReadDTO>> SignInAsync(UserSignInDTO userSignInDTO);
        Task<IValidate> UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);

    }
}
