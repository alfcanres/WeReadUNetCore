using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetPostTypeTopWithPosts : QueryStrategyBase<PostTypeReadDTO>
    {
        private readonly IQueryable<PostType> query;

        public GetPostTypeTopWithPosts(IUnitOfWork unitOfWork, IMapper mapper, int top) : base(unitOfWork, mapper)
        {
            query = unitOfWork.PostTypes.Query()
                .Include(t => t.Posts)
                .OrderByDescending(t => t.Posts.Count())
                .Take(top)
                .AsNoTracking();
        }

        internal override async Task<int> CountResults()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResults()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
