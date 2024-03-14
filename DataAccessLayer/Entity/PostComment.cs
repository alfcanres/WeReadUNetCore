using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class PostComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int ApplicationUserInfoId { set; get; }
        public ApplicationUserInfo ApplicationUserInfo { get; set; }

        public DateTime CommentDate { set; get; }

        public string CommentText { set; get; }
    }
}
