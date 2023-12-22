using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer
{
    public abstract class BaseBLL<CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly IValidate _validate = new ValidateDTO();


        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected BaseBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                //LOG
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                //LOG
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                //LOG
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                //LOG
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
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
                string friendlyError = ex.Message;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
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
                string friendlyError = ex.Message;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
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
                string friendlyError = ex.Message;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
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
                string friendlyError = ex.Message;
                _validate.MessageList.Add(friendlyError);
                _validate.IsValid = false;
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
