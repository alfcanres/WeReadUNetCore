using DataTransferObjects.Interfaces;

namespace WebAPI.Client.ViewModels
{
    public class TokenResponseViewModel : ITokenResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
