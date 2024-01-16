using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ResultResponseDTO<DTO>
    {
        private readonly DTO _DTO;
        private readonly IValidate _validate;
        public ResultResponseDTO(DTO DTO, IValidate validate)
        {
            _DTO = DTO;
            _validate = validate;
        }

        public DTO Data
        {
            get
            {
                return _DTO;
            }
        }

        public IValidate ValidateResponse
        {
            get
            {
                return _validate;
            }
        }


    }
}
