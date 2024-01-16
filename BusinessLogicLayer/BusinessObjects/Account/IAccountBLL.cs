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
        Task<ResultResponseDTO<UserReadDTO>> InsertAsync(UserCreateDTO createDTO);
        Task<ResultResponseDTO<UserReadDTO>> GetByUserNameOrEmail(string userNameOrEmail);
        public Task<ResultResponseDTO<UserReadDTO>> SignInAsync(UserSignInDTO userSignInDTO);
        Task<IValidate> UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);

    }
}
