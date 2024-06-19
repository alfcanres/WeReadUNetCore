using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IAccountBLL
    {
        Task<IResponseDTO<UserReadDTO>> InsertAsync(UserCreateDTO createDTO);
        Task<IResponseDTO<UserReadDTO>> GetByUserNameOrEmail(string userNameOrEmail);
        public Task<IValidate> SignInAsync(UserSignInDTO userSignInDTO);
        Task UpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);
        Task<IValidate> ValidateInsertAsync(UserCreateDTO createDTO);
        Task<IValidate> ValidateUpdatePasswordAsync(UserUpdatePasswordDTO updateDTO);
        

    }
}
