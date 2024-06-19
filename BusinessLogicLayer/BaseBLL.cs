using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer
{
    public abstract class BaseBLL<CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly IValidate _validate = new ValidateDTO();
        protected readonly ILogger _logger;
        private readonly IDataAnnotationsValidator _dataAnnotationsValidator;
        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected BaseBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger,
            IDataAnnotationsValidator dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _dataAnnotationsValidator = dataAnnotationsValidator;
        }



        #region Public Methods
        public async Task<IResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO)
        {
            ReadDTO readDTO = await ExecuteInsertAsync(createDTO);

            return new ResponseDTO<ReadDTO>(readDTO);
        }
        public async Task<IResponseDTO<ReadDTO>> UpdateAsync(int id, UpdateDTO updateDTO)
        {
            ReadDTO readDTO = await ExecuteUpdateAsync(updateDTO);

            return new ResponseDTO<ReadDTO>(readDTO);
        }
        public async Task DeleteAsync(int id)
        {
            await ExecuteDeleteAsync(id);
        }
        public async Task<IResponseDTO<ReadDTO>> GetByIdAsync(int id)
        {
            ReadDTO readDTO = await ExecuteGetByIdAsync(id);

            return new ResponseDTO<ReadDTO>(readDTO);
        }

        #endregion

        #region Protected Methods
        protected void ResetValidations()
        {
            _validate.Clear();
        }
        protected abstract Task<ReadDTO> ExecuteGetByIdAsync(int id);
        protected abstract Task<ReadDTO> ExecuteInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecuteDeleteAsync(int id);
        protected abstract Task<ReadDTO> ExecuteUpdateAsync(UpdateDTO updateDTO);
        protected async Task<int> CountListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await queryStrategy.CountResultsAsync();
        }

        protected async Task<IResponsePagedListDTO<ReadDTO>> ExecutePagedListAsync(QueryStrategyBase<ReadDTO> queryStrategy, IPagerDTO pager)
        {
            return await ResponsePagedListDTO<ReadDTO>.GetResponseFromQueryAsync(queryStrategy, pager);
        }

        protected async Task<IResponseListDTO<ReadDTO>> ExecuteListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await ResponseListDTO<ReadDTO>.GetResponseFromQueryAsync(queryStrategy);
        }

        protected async Task<IResponsePagedListDTO<T>> ExecutePagedListAsync<T>(QueryStrategyBase<T> queryStrategy, IPagerDTO pager)
        {
            return await ResponsePagedListDTO<T>.GetResponseFromQueryAsync(queryStrategy, pager);

        }

        protected async Task<IResponseListDTO<T>> ExecuteListAsync<T>(QueryStrategyBase<T> queryStrategy)
        {
            return await ResponseListDTO<T>.GetResponseFromQueryAsync(queryStrategy);
        }

        public async Task<IValidate> ValidateInsertAsync(CreateDTO createDTO)
        {
            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(createDTO, this._validate);

            if (this._validate.IsValid)
            {
                await ExecValidateInsertAsync(createDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);
            }

            return this._validate;
        }

        public async Task<IValidate> ValidateDeleteAsync(int id)
        {
            ResetValidations();

            await ExecValidateDeleteAsync(id);

            if (!_validate.IsValid)
                _logger.LogWarning("VALIDATE DELETE RETURNED FALSE: {id} {validate}", id, _validate);


            return this._validate;
        }

        public async Task<IValidate> ValidateUpdateAsync(int id, UpdateDTO updateDTO)
        {

            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(updateDTO, this._validate);

            if (id == 0)
            {
                _validate.AddError("Id is required");
            }

            if (this._validate.IsValid)
            {
                await ExecValidateUpdateAsync(id, updateDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);
            }

            return this._validate;
        }
        #endregion 

        #region Model Validations

        protected abstract Task ExecValidateInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecValidateDeleteAsync(int id);
        protected abstract Task ExecValidateUpdateAsync(int id, UpdateDTO updateDTO);

        #endregion

    }
}
