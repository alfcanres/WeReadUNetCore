using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.BusinessObjects
{
    public class AccountBLL : IAccountBLL
    {
        protected readonly IValidate _validate = new ValidateDTO();
        private readonly ILogger _logger;
        private readonly IUserManagerWrapper _userManager;
        private readonly IDataAnnotationsValidator _dataAnnotationsValidator;


        public AccountBLL(
            IUserManagerWrapper userManager,
            ILogger logger,
            IDataAnnotationsValidator dataAnnotationsValidator)
        {
            _userManager = userManager;
            _logger = logger;
            _dataAnnotationsValidator = dataAnnotationsValidator;
        }

        private async Task ValidateInsertAsync(UserCreateDTO createDTO)
        {
            _validate.Clear();

            _dataAnnotationsValidator.ValidateModel(createDTO, this._validate);

            if (this._validate.IsValid)
            {

                var appUser = await _userManager.FindByEmailAsync(createDTO.Email);

                if (appUser != null)
                {
                    _validate.AddError("There's already an user with that email. Please use another email");
                }

                appUser = await _userManager.FindByNameAsync(createDTO.UserName);

                if (appUser != null)
                {
                    _validate.AddError("There's already an user with that user name. Please use another user name");
                }


                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);

            }
        }
        public async Task<ResultResponseDTO<UserReadDTO>> InsertAsync(UserCreateDTO createDTO)
        {
            UserReadDTO userReadDTO = null;
            try
            {
                await ValidateInsertAsync(createDTO);

                if (this._validate.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = createDTO.UserName,
                        Email = createDTO.Email,
                        FirstName = createDTO.FirstName,
                        LastName = createDTO.LastName,
                    };

                    var result = await _userManager.CreateAsync(user, createDTO.Password);
                    if (result.Succeeded)
                    {
                        userReadDTO = new UserReadDTO
                        {
                            Email = createDTO.Email,
                            UserName = createDTO.UserName,
                            FirstName = createDTO.FirstName,
                            LastName = createDTO.LastName,
                            FullName = $"{createDTO.FirstName} {createDTO.LastName}"
                        };
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            _validate.AddError(item.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createDTO);
            }


            return new ResultResponseDTO<UserReadDTO>(userReadDTO, this._validate);
        }
        public async Task<ResultResponseDTO<UserReadDTO>> SignInAsync(UserSignInDTO userSignInDTO)
        {
            _validate.Clear();
            UserReadDTO userReadDTO = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(userSignInDTO.Email);
                if (user == null)
                {
                    _validate.AddError("Incorrect username or password.");
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(user, userSignInDTO.Password))
                    {
                        userReadDTO = new UserReadDTO
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
                        _validate.AddError("Incorrect username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "SINGIN OPERATION : {createDTO}", userSignInDTO);
            }

            return new ResultResponseDTO<UserReadDTO>(userReadDTO, this._validate);
        }
        private async Task ValidateUpdatePasswordAsync(UserUpdatePasswordDTO updateDTO)
        {
            _validate.Clear();

            _dataAnnotationsValidator.ValidateModel(updateDTO, this._validate);

            if (this._validate.IsValid)
            {

                var applicationUser = await _userManager.FindByEmailAsync(updateDTO.Email);

                var result = await _userManager.CheckPasswordAsync(applicationUser, updateDTO.OldPassword);

                if (!result)
                {
                    _validate.AddError("Incorrect password");
                }

                if (String.Compare(updateDTO.NewPassword, updateDTO.ComfirmNewPassword) != 0)
                {
                    _validate.AddError("Your new password is different than confirm new password. Please verify");
                }


                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE UPDATE PASSWORD RETURNED FALSE: {validate}", _validate);

            }
        }
        public async Task<IValidate> UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO)
        {
            try
            {
                await ValidateUpdatePasswordAsync(updateDTO);

                if (this._validate.IsValid)
                {

                    var applicationUser = await _userManager.FindByEmailAsync(updateDTO.Email);

                    var result = await _userManager.ChangePasswordAsync(applicationUser, updateDTO.OldPassword, updateDTO.NewPassword);
                    if (result.Succeeded)
                    {
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            _validate.AddError(item.Description);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "CHANGE PASSWORD OPERATION : {createDTO}", updateDTO);

            }

            return _validate;


        }
        public async Task<ResultResponseDTO<UserReadDTO>> GetByUserNameOrEmail(string userNameOrEmail)
        {
            _validate.Clear();
            UserReadDTO userDTO = null;
            var appUser = await _userManager.FindByEmailAsync(userNameOrEmail);
            if (appUser == null)
            {
                appUser = await _userManager.FindByNameAsync(userNameOrEmail);
                if (appUser != null)
                {

                    userDTO = new UserReadDTO
                    {
                        Email = appUser.Email,
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName,
                        UserName = appUser.UserName,
                        FullName = $"{appUser.FirstName} {appUser.LastName}"
                    };
                }
                else
                {
                    _validate.AddError("Incorrect user or the user doesn't exists");
                }
            }
            else
            {
                _validate.AddError("Incorrect user or the user doesn't exists");
            }

            return new ResultResponseDTO<UserReadDTO>(userDTO, this._validate);
        }

    }
}
