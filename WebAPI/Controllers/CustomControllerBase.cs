using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAPI.ExtensionMethods;

namespace WebAPI.Controllers
{
    public abstract class CustomControllerBase<CreateDTO, ReadDTO, UpdateDTO> : ControllerBase
    {
        protected readonly IBLL<CreateDTO, ReadDTO, UpdateDTO> _BLL;

        protected CustomControllerBase(IBLL<CreateDTO, ReadDTO, UpdateDTO> BLL)
        {
            _BLL = BLL;
        }

        public abstract Task<ActionResult<PagedListDTO<ReadDTO>>> GetPaged(PagerDTO pagerDTO);

        protected async Task<ActionResult<ReadDTO>> GetByIdAsync(int id)
        {
            var readModel = await _BLL.GetByIdAsync(id);
            IValidate validate = _BLL.IsOperationValid();
            if (validate.IsValid)
            {
                if (readModel == null)
                {
                    return NotFound();
                }
                else
                {
                    return readModel;
                }
            }
            else
            {
                return StatusCode(500, validate);
            }
        }

        protected async Task<ActionResult> CreateAsync(CreateDTO createDTO)
        {
            IValidate validate = null;
            if (!TryValidateModel(createDTO))
            {
                validate = ModelState.ToValidate();
            }
            validate = await _BLL.ValidateInsertAsync(createDTO);
            if (validate.IsValid)
            {
                var readModel = await _BLL.InsertAsync(createDTO);
                validate = _BLL.IsOperationValid();
                if (!validate.IsValid)
                    return StatusCode(500, validate);

                return Ok(readModel);
            }
            else
            {
                return BadRequest(validate);
            }
        }

        protected async Task<ActionResult> UpdateAsync(UpdateDTO updateDTO)
        {
            IValidate validate = null;
            if (!TryValidateModel(updateDTO))
            {
                validate = ModelState.ToValidate();
                return BadRequest(validate);
            }

            validate = await _BLL.ValidateUpdateAsync(updateDTO);
            if (validate.IsValid)
            {
                await _BLL.UpdateAsync(updateDTO);
                validate = _BLL.IsOperationValid();
                if (!validate.IsValid)
                    return StatusCode(500, validate);
                return NoContent();
            }
            else
            {
                return BadRequest(validate);
            }
        }

        protected async Task<ActionResult> DeleteAsync(int id)
        {
            IValidate validate = null;
            validate = await _BLL.ValidateDeleteAsync(id);
            if (validate.IsValid)
            {
                await _BLL.DeleteAsync(id);
                validate = _BLL.IsOperationValid();
                if (!validate.IsValid)
                    return StatusCode(500, validate);
                return NoContent();
            }
            else
            {
                return BadRequest(validate);
            }
        }


    }
}
