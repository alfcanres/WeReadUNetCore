using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

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
