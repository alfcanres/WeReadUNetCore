using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Response
{
    public class ResponseDTO<T> : IResponseDTO<T> where T : class
    {
        private readonly T _DTO;
        private readonly IValidate _validate;
        public ResponseDTO(T DTO, IValidate validate)
        {
            _DTO = DTO;
            _validate = validate;
        }

        public T Data
        {
            get
            {
                return _DTO;
            }
        }

        public IValidate Validate
        {
            get
            {
                return _validate;
            }
        }
    }
}
