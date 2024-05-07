using AutoMapper;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        protected void ResetValidations()
        {
            _validate.Clear();
        }

        #region CRUDS
        public async Task<IResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO)
        {

            ReadDTO readDTO = null;

            try
            {
                await ValidateInsertAsync(createDTO);
                if (this._validate.IsValid)
                {
                    var entity = Mapper.Map<TEntity>(createDTO);
                    await _repository.InsertAsync(entity);
                    readDTO = Mapper.Map<ReadDTO>(entity);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createDTO);

            }

            return new ResponseDTO<ReadDTO>(readDTO, this._validate);
        }
        public async Task<IResponseDTO<ReadDTO>> UpdateAsync(int id, UpdateDTO updateDTO)
        {
            ReadDTO readDTO = null;

            try
            {
                await ValidateUpdateAsync(updateDTO);
                if (this._validate.IsValid)
                {
                    var entity = await _repository.GetByIdAsync(id);
                    Mapper.Map(updateDTO, entity);
                    await _repository.UpdateAsync(entity);
                    readDTO = Mapper.Map<ReadDTO>(entity);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnUpdateOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "UPDATE OPERATION : {updateDTO}", updateDTO);
            }

            return new ResponseDTO<ReadDTO>(readDTO, this._validate);
        }
        public async Task<IValidate> DeleteAsync(int id)
        {
            try
            {
                await ValidateDeleteAsync(id);
                if (this._validate.IsValid)
                {
                    await _repository.DeleteAsync(id);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnDeleteOperation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "DELETE OPERATION : {id}", id);
            }

            return this._validate;
        }
        public async Task<IResponseDTO<ReadDTO>> GetByIdAsync(int id)
        {
            ResetValidations();
            ReadDTO readDTO = null;
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                readDTO = Mapper.Map<ReadDTO>(entity);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "GET BY ID OPERATION : {id}", id);
            }

            return new ResponseDTO<ReadDTO>(readDTO, this._validate);
        }

        protected async Task<int> CountListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            ResetValidations();
            try
            {
                return await queryStrategy.CountResultsAsync();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE COUNT ERROR {queryStrategy}", queryStrategy);
                return 0;
            }
        }

        protected async Task<IResponsePagedListDTO<ReadDTO>> ExecutePagedListAsync(QueryStrategyBase<ReadDTO> queryStrategy, IPagerDTO pager)
        {
            ResetValidations();
            try
            {

                return await ResponsePagedListDTO<ReadDTO>.GetResponseFromQueryAsync(queryStrategy, pager, this._validate);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR {queryStrategy}", queryStrategy);
                return new ResponsePagedListDTO<ReadDTO>(this._validate);
            }
        }

        protected async Task<IResponseListDTO<ReadDTO>> ExecuteListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            ResetValidations();
            try
            {
                return await ResponseListDTO<ReadDTO>.GetResponseFromQueryAsync(queryStrategy, this._validate);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR {queryStrategy}", queryStrategy);
                return new ResponseListDTO<ReadDTO>(this._validate);
            }
        }

        protected async Task<IResponsePagedListDTO<T>> ExecutePagedListAsync<T>(QueryStrategyBase<T> queryStrategy, IPagerDTO pager)
        {
            ResetValidations();
            try
            {

                return await ResponsePagedListDTO<T>.GetResponseFromQueryAsync(queryStrategy, pager, this._validate);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR {queryStrategy}", queryStrategy);
                return new ResponsePagedListDTO<T>(this._validate);
            }
        }

        protected async Task<IResponseListDTO<T>> ExecuteListAsync<T>(QueryStrategyBase<T> queryStrategy)
        {
            ResetValidations();
            try
            {
                return await ResponseListDTO<T>.GetResponseFromQueryAsync(queryStrategy, this._validate);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR {queryStrategy}", queryStrategy);
                return new ResponseListDTO<T>(this._validate);
            }
        }


        #endregion

        #region Model Validations

        protected abstract Task ExecValidateInsertAsync(CreateDTO createDTO);
        private async Task ValidateInsertAsync(CreateDTO createDTO)
        {
            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(createDTO, this._validate);

            if (this._validate.IsValid)
            {
                await ExecValidateInsertAsync(createDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);
            }
        }
        protected abstract Task ExecValidateDeleteAsync(int id);
        private async Task ValidateDeleteAsync(int id)
        {
            ResetValidations();

            await ExecValidateDeleteAsync(id);

            if (!_validate.IsValid)
                _logger.LogWarning("VALIDATE DELETE RETURNED FALSE: {id} {validate}", id, _validate);

        }
        protected abstract Task ExecValidateUpdateAsync(UpdateDTO updateDTO);
        private async Task ValidateUpdateAsync(UpdateDTO updateDTO)
        {

            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(updateDTO, this._validate);

            if (this._validate.IsValid)
            {
                await ExecValidateUpdateAsync(updateDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);
            }

        }


        #endregion

    }
}
