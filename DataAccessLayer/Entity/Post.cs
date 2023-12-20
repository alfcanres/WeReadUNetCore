using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }

        public int MoodTypeId { get; set; }

        public MoodType MoodType { get; set; }

        [MaxLength(1024, ErrorMessage = "Avoid TDL!!! Max chars 1024")]
        [Required(ErrorMessage = "Need some text here!")]
        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public Nullable<DateTime> PublishDate { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<PostVote> Votes { set; get; }




    }
}
