using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer
{
    public abstract class BaseBLL<CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly IValidate _validate = new ValidateDTO();
        private readonly ILogger _logger; 
        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected BaseBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        protected void ResetValidations()
        {
            _validate.Clear();
        }

        #region CRUDS
        public async Task<ReadDTO> InsertAsync(CreateDTO createDTO)
        {
            ResetValidations();
            try
            {
                return await ExecuteInsertAsync(createDTO);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createDTO);
                return default(ReadDTO);
            }
        }
        public async Task<ReadDTO> UpdateAsync(UpdateDTO updateDTO)
        {
            ResetValidations();
            try
            {
                return await ExecuteUpdateAsync(updateDTO);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnUpdateOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "UPDATE OPERATION : {updateDTO}", updateDTO);
                return default(ReadDTO);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            ResetValidations();
            try
            {
                return await ExecuteDeleteAsync(id);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnDeleteOperation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "DELETE OPERATION : {id}", id);
                return false;
            }
        }
        public async Task<ReadDTO> GetByIdAsync(int id)
        {
            ResetValidations();
            try
            {
                return await ExecuteGetByIdAsync(id);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "GET BY ID OPERATION : {id}", id);
                return default(ReadDTO);
            }

        }

        protected abstract Task<ReadDTO> ExecuteGetByIdAsync(int id);
        protected abstract Task<bool> ExecuteDeleteAsync(int id);
        protected abstract Task<ReadDTO> ExecuteInsertAsync(CreateDTO createDTO);
        protected abstract Task<ReadDTO> ExecuteUpdateAsync(UpdateDTO updateDTO);
        protected async Task<IEnumerable<ReadDTO>> ExecuteListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            ResetValidations();
            try
            {
                return await queryStrategy.GetResults();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR {queryStrategy}", queryStrategy);
                return null;
            }
        }
        protected async Task<int> CountListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            ResetValidations();
            try
            {
                return await queryStrategy.CountResults();
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

        #endregion

        #region Model Validations
        protected abstract Task<IValidate> ExecValidateInsertAsync(CreateDTO createDTO);
        public async Task<IValidate> ValidateInsertAsync(CreateDTO createDTO)
        {
            ResetValidations();

            try
            {
                await ExecValidateInsertAsync(createDTO);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
                _logger.LogError(ex, "VALIDATE INSERT OPERATION : {createDTO}", createDTO);
            }
            return _validate;
        }
        protected abstract Task<IValidate> ExecValidateDeleteAsync(int id);
        public async Task<IValidate> ValidateDeleteAsync(int id)
        {
            ResetValidations();
            try
            {
                await ExecValidateDeleteAsync(id);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnDeleteOperation;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
                _logger.LogError(ex, "VALIDATE DELETE OPERATION : {id}", id);
            }
            return _validate;
        }
        protected abstract Task<IValidate> ExecValidateUpdateAsync(UpdateDTO updateDTO);
        public async Task<IValidate> ValidateUpdateAsync(UpdateDTO updateDTO)
        {

            ResetValidations();

            try
            {
                await ExecValidateUpdateAsync(updateDTO);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnUpdateOpeation;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
                _logger.LogError(ex, "VALIDATE UPDATE OPERATION : {updateDTO}", updateDTO);
            }
            return _validate;
        }

        public IValidate IsOperationValid()
        {
            return _validate;
        }

        #endregion

    }
}
