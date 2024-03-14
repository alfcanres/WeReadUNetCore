using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class PostVote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int ApplicationUserInfoId { set; get; }
        public ApplicationUserInfo ApplicationUserInfo { get; set; }
        public bool ILikedThis { get; set; }
        public DateTime VoteDate { get; set; }
    }
}
