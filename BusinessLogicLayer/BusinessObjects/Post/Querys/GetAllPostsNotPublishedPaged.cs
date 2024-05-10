using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostsNotPublished : QueryStrategyBase<PostPendingToPublishDTO>
    {
        private readonly IQueryable<Post> query;
        public GetAllPostsNotPublished(IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager) : base(unitOfWork, mapper)
        {
            query = unitOfWork.Posts
                .Query()
                .AsNoTracking()
                .Where(t => !t.IsPublished)
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderBy(t => t.CreationDate);
                

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostPendingToPublishDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
