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
        public int MoodTypeId { init; get; }
        public int PostTypeId { init; get; }
        public string ProfilePic { init; get; }
        public string PostType { get; init; }
        public string MoodType { get; init; }
        public string Title { init; get; }
        public string Text { get; init; }
    }
}
