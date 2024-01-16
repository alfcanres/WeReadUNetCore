using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


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
        private readonly ILogger _logger;
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

        protected void ResetValidations()
        {
            _validate.Clear();
        }

        #region CRUDS
        public async Task<ResultResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO)
        {

            ReadDTO readDTO = null;

            try
            {
                await ValidateInsertAsync(createDTO);
                if (this._validate.IsValid)
                {
                    readDTO = await ExecuteInsertAsync(createDTO);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnInsertOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createDTO);

            }

            return new ResultResponseDTO<ReadDTO>(readDTO, this._validate);
        }
        public async Task<ResultResponseDTO<ReadDTO>> UpdateAsync(UpdateDTO updateDTO)
        {
            ReadDTO readDTO = null;

            try
            {
                await ValidateUpdateAsync(updateDTO);
                if (this._validate.IsValid)
                {
                    readDTO = await ExecuteUpdateAsync(updateDTO);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnUpdateOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "UPDATE OPERATION : {updateDTO}", updateDTO);
            }

            return new ResultResponseDTO<ReadDTO>(readDTO, this._validate);
        }
        public async Task<IValidate> DeleteAsync(int id)
        {
            try
            {
                await ValidateDeleteAsync(id);
                if (this._validate.IsValid)
                {
                    await ExecuteDeleteAsync(id);
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
        public async Task<ResultResponseDTO<ReadDTO>> GetByIdAsync(int id)
        {
            ResetValidations();
            ReadDTO readDTO = null;
            try
            {
                readDTO = await ExecuteGetByIdAsync(id);
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "GET BY ID OPERATION : {id}", id);
            }

            return new ResultResponseDTO<ReadDTO>(readDTO, this._validate);
        }
        protected abstract Task<ReadDTO> ExecuteGetByIdAsync(int id);
        protected abstract Task<ReadDTO> ExecuteInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecuteDeleteAsync(int id);
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
