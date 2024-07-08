using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record TokenResponse : ITokenResponse
    {
        public string Token { init; get; }
        public DateTime Expires { init; get; }
    }
}
