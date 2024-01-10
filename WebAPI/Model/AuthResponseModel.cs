using System.Runtime.CompilerServices;

namespace WebAPI.Model
{
    public class AuthResponseModel
    {
        private readonly string _token;
        private readonly DateTime _expires;

        public AuthResponseModel(string token, DateTime expires)
        {
            _token = token;
            _expires = expires;
        }

        public string Token => _token;

        public DateTime Expires => _expires;
    }
}
