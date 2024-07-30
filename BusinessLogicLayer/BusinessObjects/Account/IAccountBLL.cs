using DataTransferObjects;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IAccountBLL
    {
        Task<UserRegisteredDTO> InsertAsync(UserCreateDTO createDTO);
        Task<UserReadDTO> GetByUserNameOrEmail(string userNameOrEmail);
        public Task<ValidatorResponse> SignInAsync(UserSignInDTO userSignInDTO);
        Task UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);
        Task<ValidatorResponse> ValidateInsertAsync(UserCreateDTO createDTO);
        Task<ValidatorResponse> ValidateUpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);
        

    }
}
