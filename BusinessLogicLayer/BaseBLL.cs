using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer
{
    public abstract class BaseBLL<CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly IValidate _validate;


        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected BaseBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidate validate)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validate = validate;
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
        public abstract Task<ReadDTO> GetByIdAsync(int id);
        protected abstract Task<bool> ExecuteDeleteAsync(int id);
        protected abstract Task<ReadDTO> ExecuteInsertAsync(CreateDTO createDTO);
        protected abstract Task<ReadDTO> ExecuteUpdateAsync(UpdateDTO updateDTO);
        protected async Task<IEnumerable<ReadDTO>> ExecuteListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await queryStrategy.GetResults();
        }
        protected async Task<int> CountListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await queryStrategy.CountResults();
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

        #endregion

    }
}
