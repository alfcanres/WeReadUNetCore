using System.ComponentModel.DataAnnotations;

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

        [Required]
        [MaxLength(255)]
        public string CommentText { set; get; }
    }
}
