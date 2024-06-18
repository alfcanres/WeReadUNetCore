using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer.BusinessObjects
{
    public class AccountBLL : IAccountBLL
    {
        protected readonly IValidate _validate = new ValidateDTO();
        private readonly ILogger<AccountBLL> _logger;
        private readonly IUserManagerWrapper _userManager;
        private readonly IDataAnnotationsValidator _dataAnnotationsValidator;
        private readonly IUnitOfWork _unitOfWork;


        public AccountBLL(
            IUserManagerWrapper userManager,
            ILogger<AccountBLL> logger,
            IDataAnnotationsValidator dataAnnotationsValidator,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _logger = logger;
            _dataAnnotationsValidator = dataAnnotationsValidator;
            _unitOfWork = unitOfWork;
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
                    _validate.AddError(ValidationAccountErrorMessages.OnInsertEmailAlreadyInUse);
                }

                appUser = await _userManager.FindByNameAsync(createDTO.UserName);

                if (appUser != null)
                {
                    _validate.AddError(ValidationAccountErrorMessages.OnInsertUserNameAlreadyInUse);
                }


                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);

            }
        }
        public async Task<IResponseDTO<UserReadDTO>> InsertAsync(UserCreateDTO createDTO)
        {
            UserReadDTO userReadDTO = null;
            try
            {
                await ValidateInsertAsync(createDTO);

                if (this._validate.IsValid)
                {
                    var user = new IdentityUser
                    {
                        UserName = createDTO.UserName,
                        Email = createDTO.Email,
                    };

                    var result = await _userManager.CreateAsync(user, createDTO.Password);
                    if (result.Succeeded)
                    {

                        ApplicationUserInfo applicationUserInfo = new ApplicationUserInfo()
                        {
                            FirstName = createDTO.UserName,
                            LastName = createDTO.LastName,
                            UserID = user.Id,
                            UserName = createDTO.UserName,
                            ProfilePicture = "Default",
                            DateOfBirth = DateTime.Now,
                            IsActive = true
                        };



                        await _unitOfWork.UsersInfo.InsertAsync(applicationUserInfo);


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


            return new ResponseDTO<UserReadDTO>(userReadDTO, this._validate);
        }
        public async Task<IResponseDTO<UserReadDTO>> SignInAsync(UserSignInDTO userSignInDTO)
        {
            _validate.Clear();
            UserReadDTO userReadDTO = null;
            try
            {
                var user = await _userManager.FindByEmailAsync(userSignInDTO.Email);
                if (user == null)
                {
                    _validate.AddError(ValidationAccountErrorMessages.OnLoginIncorrectUserNameOrPassword);
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(user, userSignInDTO.Password))
                    {


                        var userInfo = await GetUserInfoAsync(user);


                        userReadDTO = new UserReadDTO
                        {
                            Email = userSignInDTO.Email,
                            UserName = user.UserName,
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            FullName = $"{userInfo.FirstName} {userInfo.LastName}",
                            ProfilePicture = userInfo.ProfilePicture
                        };

                    }
                    else
                    {
                        _validate.AddError(ValidationAccountErrorMessages.OnLoginIncorrectUserNameOrPassword);
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

            return new ResponseDTO<UserReadDTO>(userReadDTO, this._validate);
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
                    _validate.AddError(ValidationAccountErrorMessages.OnUpdatePasswordWrongPassword);
                }

                if (String.Compare(updateDTO.NewPassword, updateDTO.ComfirmNewPassword) != 0)
                {
                    _validate.AddError(ValidationAccountErrorMessages.OnUpdatePasswordIncorretConfirmPassword);
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
                    if (!result.Succeeded)
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
        public async Task<IResponseDTO<UserReadDTO>> GetByUserNameOrEmail(string userNameOrEmail)
        {
            _validate.Clear();
            UserReadDTO userDTO = null;
            var identityUser = await _userManager.FindByNameAsync(userNameOrEmail);

            if (identityUser != null)
            {
                var userInfo = await GetUserInfoAsync(identityUser);

                userDTO = new UserReadDTO
                {
                    Email = identityUser.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    UserName = userInfo.UserName,
                    FullName = $"{userInfo.FirstName} {userInfo.LastName}"
                };
            }
            else
            {
                _validate.AddError("Incorrect user or the user doesn't exists");
            }

            return new ResponseDTO<UserReadDTO>(userDTO, this._validate);
        }
        private async Task<(string UserName, string FirstName, string LastName, string ProfilePicture)> GetUserInfoAsync(IdentityUser user)
        {
            var userInfo = await _unitOfWork.UsersInfo.Query().FirstOrDefaultAsync(t => t.UserID == user.Id);
            string firstName = "";
            string lastName = "";
            string profilePicture = "";
            string userName = user.Email;

            if (userInfo is ApplicationUserInfo)
            {
                firstName = userInfo.FirstName;
                lastName = userInfo.LastName;
                profilePicture = userInfo.ProfilePicture;
                userName = userInfo.UserName;
            }

            return (userName, firstName, lastName, profilePicture);

        }

    }
}
