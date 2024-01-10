using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public class UserUpdateDTO
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid user name")]
        public string UserName { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid email")]
        public string FirstName { init; get; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Type a valid email")]
        public string LastName { init; get; }

        [EmailAddress]
        [Required(ErrorMessage = "Type a valid email")]
        public string Email { init; get; }
    }
}
