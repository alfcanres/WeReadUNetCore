﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class ApplicationUserInfo
    {
        public int Id { set; get; }
        public string? UserID { set; get; }

        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your username")]
        public string UserName { set; get; }
        public string? ProfilePicture { get; set; }

        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your first name")]
        public string FirstName { get; set; }
        [MaxLength(255, ErrorMessage = "Max chars 255")]
        [Required(ErrorMessage = "Please type your last name")]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public bool IsActive { get; set; }
        public ICollection<PostVote> Votes { get; set; }
        public ICollection<PostComment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
