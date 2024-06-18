using DataTransferObjects;

namespace WebApp.Models
{
    public class TokenResponseViewModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public ValidateDTO Validate { get; set; }
    }
}
