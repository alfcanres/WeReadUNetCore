using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class AccountBLL : IAccountBLL
    {
        protected readonly ValidatorResponse _validate = new ValidatorResponse();
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

        public async Task<ValidatorResponse> ValidateInsertAsync(UserCreateDTO createDTO)
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
                else
                {
                    appUser = await _userManager.FindByNameAsync(createDTO.UserName);

                    if (appUser != null)
                    {
                        _validate.AddError(ValidationAccountErrorMessages.OnInsertUserNameAlreadyInUse);
                    }
                }


                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);

            }

            return this._validate;
        }
        public async Task<UserRegisteredDTO> InsertAsync(UserCreateDTO createDTO)
        {

            UserRegisteredDTO registeredUser = new UserRegisteredDTO();

            ValidatorResponse validateDTO = new ValidatorResponse();
            validateDTO.IsValid = true;
            validateDTO.MessageList = new List<string>();

            registeredUser.ValidateDTO = validateDTO;

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

                registeredUser.Email = createDTO.Email;
                registeredUser.UserName = createDTO.UserName;
            }
            else
            {
                result.Errors.ToList().ForEach(t => _validate.AddError(t.Description));
            }

            return registeredUser;
        }
        public async Task<ValidatorResponse> SignInAsync(UserSignInDTO userSignInDTO)
        {
            _validate.Clear();

            var user = await _userManager.FindByEmailAsync(userSignInDTO.Email);
            if (user == null)
            {
                _validate.AddError(ValidationAccountErrorMessages.OnLoginIncorrectUserNameOrPassword);
            }
            else
            {
                if (await _userManager.CheckPasswordAsync(user, userSignInDTO.Password))
                {
                    this._validate.IsValid = true;
                }
                else
                {
                    _validate.AddError(ValidationAccountErrorMessages.OnLoginIncorrectUserNameOrPassword);
                }
            }

            return this._validate;
        }
        public async Task<ValidatorResponse> ValidateUpdatePasswordAsync(UserUpdatePasswordDTO updateDTO)
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

            return this._validate;
        }
        public async Task UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO)
        {
            var applicationUser = await _userManager.FindByEmailAsync(updateDTO.Email);

            var result = await _userManager.ChangePasswordAsync(applicationUser, updateDTO.OldPassword, updateDTO.NewPassword);
        }
        public async Task<UserReadDTO> GetByUserNameOrEmail(string userNameOrEmail)
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

            return userDTO;
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
