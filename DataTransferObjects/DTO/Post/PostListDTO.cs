using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.DTO.Post
{
    public record PostListDTO
    {
        public int Id { get; init; }
        public string ApplicationUserId { init; get; }
        public string UserName { init; get; }
        public string ProfilePic { init; get; }
        public string PostType { get; init; }
        public string MoodType { get; init; }
        public string Title { init; get; }
        public int Votes { init; get; }

        public int Comments { init; get; }

    }
}
