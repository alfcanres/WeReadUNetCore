using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public class UserSignInDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }

        [Required(ErrorMessage = "Type a valid password")]
        [DataType(DataType.Password)]
        public string Password { init; get; }
    }
}
