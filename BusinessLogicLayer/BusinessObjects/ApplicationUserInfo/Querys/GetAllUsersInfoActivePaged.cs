using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObject
{
    public class GetAllUsersInfoActivePaged : QueryStrategyBase<ApplicationUserInfoListDTO>
    {

        private readonly IQueryable<ApplicationUserInfo> query;

        public GetAllUsersInfoActivePaged(IUnitOfWork unitOfWork, IMapper mapper, IPagerDTO pager)
            : base(unitOfWork, mapper)
        {
            query = unitOfWork.UsersInfo
                .Query()
                .Where(t => t.IsActive)
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .AsNoTracking();
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<ApplicationUserInfoListDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
