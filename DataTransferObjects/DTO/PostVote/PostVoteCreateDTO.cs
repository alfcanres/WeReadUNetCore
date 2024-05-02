using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostVoteCreateDTO
    {
        public int PostId { get; init; }
        public int ApplicationUserInfoId { get; init; }
    }
}
