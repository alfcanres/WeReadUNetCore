using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO.Post;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetPostsPublishedByUserPaged : QueryStrategyBase<PostListDTO>
    {
        private readonly IQueryable<Post> queryList;
        private readonly IQueryable<Post> queryCount;
        public GetPostsPublishedByUserPaged(int UserID, IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager) : base(unitOfWork, mapper)
        {
            var baseQuery = unitOfWork.Posts
                 .Query()
                 .AsNoTracking();

            if (String.IsNullOrEmpty(pager.SearchKeyWord))
            {
                baseQuery = baseQuery
                    .Where(t =>
                    t.Title.Contains(pager.SearchKeyWord)
                    ||
                    t.MoodType.Mood.Contains(pager.SearchKeyWord)
                    ||
                    t.PostType.Description.Contains(pager.SearchKeyWord)
                    );
            }

            baseQuery = baseQuery
                .OrderBy(t => t.CreationDate)
                .Where(t => t.IsPublished && t.ApplicationUserInfoId == UserID);

            
            queryCount = baseQuery;

            queryList = baseQuery.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<PostListDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
