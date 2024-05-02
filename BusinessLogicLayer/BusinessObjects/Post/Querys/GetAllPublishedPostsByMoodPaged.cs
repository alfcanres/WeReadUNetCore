using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPublishedPostsByMoodPaged : QueryStrategyBase<PostReadDTO>
    {
        private readonly IQueryable<Post> query;
        public GetAllPublishedPostsByMoodPaged(IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager, int moodTypeId) : base(unitOfWork, mapper)
        {
            query = unitOfWork.Posts
                .Query()
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderBy(t => t.CreationDate)
                .Where(t => t.IsPublished && t.MoodTypeId == moodTypeId)
                .AsNoTracking();
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
