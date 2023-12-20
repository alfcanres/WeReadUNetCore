using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostTypes : QueryStrategyBase<PostTypeReadDTO>
    {
        public GetAllPostTypes(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        internal override async Task<int> CountResults()
        {
            return await unitOfWork.PostTypes.Query().CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResults()
        {
            var result = await unitOfWork.PostTypes.Query().ToListAsync();
          
            return Map(result);
        }
    }
}
