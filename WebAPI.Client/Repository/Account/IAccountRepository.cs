using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Account
{
    public interface IAccountRepository
    {
        Task<ResponseViewModel<TokenResponse>> LoginAsync(UserSignInDTO loginViewModel);
        Task<ResponseViewModel<TokenResponse>> RefreshToken(string expiredToken);
        Task<ResponseViewModel<bool>> RegisterAsync(UserCreateDTO createModel);
    }
}
