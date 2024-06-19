using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.Extensions.Logging;
namespace BusinessLogicLayer
{
    public abstract class CRUDBaseBLL<TEntity, CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        #region Properties
        private IRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        protected readonly IValidate _validate = new ValidateDTO();
        protected readonly ILogger _logger;
        private readonly IDataAnnotationsValidator _dataAnnotationsValidator;
        protected IRepository<TEntity> Repository { get { return _repository; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected CRUDBaseBLL(
            IRepository<TEntity> repository,
            IMapper mapper,
            ILogger logger,
            IDataAnnotationsValidator dataAnnotationsValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _dataAnnotationsValidator = dataAnnotationsValidator;
        }


        #region Public Methods
        public async Task<IResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO)
        {
            var entity = Mapper.Map<TEntity>(createDTO);
            await _repository.InsertAsync(entity);
            ReadDTO readDTO = Mapper.Map<ReadDTO>(entity);

            return new ResponseDTO<ReadDTO>(readDTO);
        }
        public async Task<IResponseDTO<ReadDTO>> UpdateAsync(int id, UpdateDTO updateDTO)
        {

            var entity = await _repository.GetByIdAsync(id);
            Mapper.Map(updateDTO, entity);
            await _repository.UpdateAsync(entity);
            ReadDTO readDTO = Mapper.Map<ReadDTO>(entity);

            return new ResponseDTO<ReadDTO>(readDTO);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<IResponseDTO<ReadDTO>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            ReadDTO readDTO = Mapper.Map<ReadDTO>(entity);

            return new ResponseDTO<ReadDTO>(readDTO);
        }
        #endregion

        #region Protected Methods
        protected void ResetValidations()
        {
            _validate.Clear();
        }

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

        protected abstract Task ExecValidateInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecValidateDeleteAsync(int id);
        protected abstract Task ExecValidateUpdateAsync(int id, UpdateDTO updateDTO);

        #endregion

        #region Model Validations

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
                _validate.AddError("Id is required.");

            if (this._validate.IsValid)
            {
                await ExecValidateUpdateAsync(id, updateDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);
            }

            return this._validate;
        }

        #endregion

    }
}
