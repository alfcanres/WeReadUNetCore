using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record UserRegisteredDTO
    {
        public string UserName { set; get; }
        public string Email { set; get; }

        public ValidatorResponse ValidateDTO { set; get; }
    }
}
