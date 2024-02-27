using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO
{
    public record PostReadDTO
    {
        public int Id { get; init; }
        public string ApplicationUserId { init; get; }

        public string UserName { init; get; }
        public string ProfilePic {  init; get; }
        public string PostType { get; init; }
        public string MoodType { get; init; }

        [MaxLength(1024, ErrorMessage = "Avoid TDL!!! Max chars 1024")]
        [Required(ErrorMessage = "Need some text here!")]
        public string Text { get; init; }
    }
}
