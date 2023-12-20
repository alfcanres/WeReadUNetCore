using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public abstract class QueryStrategyBase<ReadDTO>
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        protected QueryStrategyBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        internal abstract Task<IEnumerable<ReadDTO>> GetResults();

        internal abstract Task<int> CountResults();

        protected IEnumerable<ReadDTO> Map(object list)
        {
            return mapper.Map<List<ReadDTO>>(list);
        }
       

    }
}
