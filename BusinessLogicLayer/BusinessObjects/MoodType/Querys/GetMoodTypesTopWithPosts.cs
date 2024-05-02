using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObjects
{
    public class GetMoodTypesTopWithPosts : QueryStrategyBase<MoodTypeReadDTO>
    {
        private readonly IQueryable<MoodType> query;
        public GetMoodTypesTopWithPosts(IUnitOfWork unitOfWork, IMapper mapper, int top) : base(unitOfWork, mapper)
        {
            query = unitOfWork.MoodTypes
                .Query()
                .Take(top)
                .OrderByDescending(t => t.Mood)
                .AsNoTracking();
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
