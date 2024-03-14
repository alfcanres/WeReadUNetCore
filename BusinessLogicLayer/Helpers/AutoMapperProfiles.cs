using AutoMapper;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<PostTypeCreateDTO, PostType>();
            CreateMap<PostTypeUpdateDTO, PostType>();
            CreateMap<PostType, PostTypeReadDTO>()
                 .ForMember(t => t.PostCount, t => t.MapFrom(x => x.Posts.Count))
                 .ReverseMap();

            CreateMap<MoodTypeCreateDTO, MoodType>();
            CreateMap<MoodTypeUpdateDTO, MoodType>();
            CreateMap<MoodType, MoodTypeReadDTO>()
                 .ForMember(t => t.PostCount, t => t.MapFrom(x => x.Posts.Count))
                 .ReverseMap();



            CreateMap<PostCreateDTO, Post>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<Post, PostReadDTO>()
                 .ReverseMap();

            CreateMap<Post, PostPendingToublishDTO>()
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ForMember(t => t.ProfilePic, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.PostType, t => t.MapFrom(m => m.PostType.Description))
                .ForMember(t => t.MoodType, t => t.MapFrom(m => m.MoodType.Mood));

            CreateMap<Post, PostListDTO>()
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ForMember(t => t.ProfilePic, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.PostType, t => t.MapFrom(m => m.PostType.Description))
                .ForMember(t => t.MoodType, t => t.MapFrom(m => m.MoodType.Mood))
                .ForMember(t => t.Votes, t => t.MapFrom(m => m.Votes.Count()))
                .ForMember(t => t.Comments, t => t.MapFrom(m => m.Comments.Count()));



        }
    }
}
