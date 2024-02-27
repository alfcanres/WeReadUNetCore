
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Model
{
    public class TokenResponseModel
    {
        private readonly string _token;
        private readonly DateTime _expires;
        private readonly IResponseDTO<UserReadDTO> _resultResponseDTO;
        private readonly IConfiguration _configuration;
        private readonly IValidate _validate;


        public TokenResponseModel(IResponseDTO<UserReadDTO> resultResponseDTO, DateTime expires, IConfiguration configuration)
        {
            _resultResponseDTO = resultResponseDTO;
            _configuration = configuration;
            _validate = resultResponseDTO.Validate;
            if (_validate.IsValid)
            {
                _token = GenerateToken(_resultResponseDTO.Data);
                _expires = expires;
            }
            else
            {
                _token = "";
                _expires = DateTime.Now.AddDays(-1);
            }

        }

        public string Token => _token;

        public DateTime Expires => _expires;

        public IValidate Validate => _validate;

        private string GenerateToken(UserReadDTO userReadDTO)
        {

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userReadDTO.Email),
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("jwt:Key").Value)
                );

            SigningCredentials signingCredentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
            );


            var securityToken = new JwtSecurityToken(
            claims: claims,
                expires: DateTime.Now.AddDays(1),
                issuer: _configuration.GetSection("jwt:Issuer").Value,
                audience: _configuration.GetSection("jwt:Audience").Value,
                signingCredentials: signingCredentials
                );


            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
