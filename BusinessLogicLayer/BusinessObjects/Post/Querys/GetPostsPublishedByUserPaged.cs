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
        private readonly IQueryable<Post> query;
        public GetPostsPublishedByUserPaged(int UserID, IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager) : base(unitOfWork, mapper)
        {
            query = unitOfWork.Posts
                .Query();

            if (String.IsNullOrEmpty(pager.SearchKeyWord))
            {
                query = query
                    .Where(t =>
                    t.Title.Contains(pager.SearchKeyWord)
                    ||
                    t.MoodType.Mood.Contains(pager.SearchKeyWord)
                    ||
                    t.PostType.Description.Contains(pager.SearchKeyWord)
                    );
            }

            query = query.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderBy(t => t.CreationDate)
                .Where(t => t.IsPublished && t.ApplicationUserInfoId == UserID)
                .AsNoTracking();

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostListDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
