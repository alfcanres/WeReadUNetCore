using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllMoodTypesPaged : QueryStrategyBase<MoodTypeReadDTO>
    {
        private readonly IQueryable<MoodType> query;

        public GetAllMoodTypesPaged(IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager)
            : base(unitOfWork, mapper)
        {
            query = unitOfWork.MoodTypes
                .Query()
                .AsNoTracking()
                .Where(t => t.IsAvailable)
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await unitOfWork.MoodTypes.Query().CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
