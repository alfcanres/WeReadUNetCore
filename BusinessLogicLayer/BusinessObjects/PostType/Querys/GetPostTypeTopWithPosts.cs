using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

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

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
