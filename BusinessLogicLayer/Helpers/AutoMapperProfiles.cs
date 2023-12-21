using AutoMapper;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
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

        }
    }
}
