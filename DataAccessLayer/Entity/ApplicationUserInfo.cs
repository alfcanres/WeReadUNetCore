using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class ApplicationUserInfo
    {
        public int Id { set; get; }
        public string? UserID { set; get; }
        public string UserName { set; get; }
        public string? ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        ICollection<PostVote> Votes { get; set; }
        ICollection<PostComment> Comments { get; set; }
        ICollection<Post> Posts { get; set; }
    }
}
