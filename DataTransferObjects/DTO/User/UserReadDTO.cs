using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record UserReadDTO
    {
        public string UserName { init; get; }

        public string ProfilePicture { init; get; }

        public string FullName { init; get; }
        public string FirstName { init; get; }
        public string LastName { init; get; }
        public string Email { init; get; }
    }
}
