using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostTypePaged : QueryStrategyBase<PostTypeReadDTO>
    {

        private readonly IQueryable<PostType> query;

        public GetAllPostTypePaged(IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager) : base(unitOfWork, mapper)
        {
            query = unitOfWork.PostTypes
                .Query()
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .AsNoTracking();
        }
        internal override async Task<int> CountResultsAsync()
        {
            return await unitOfWork.PostTypes.Query().CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
