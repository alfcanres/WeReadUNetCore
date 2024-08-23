using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostVoteResultDTO
    {
        public int Id { get; init; }

        public string PostTitle { get; init; }


    }
}
