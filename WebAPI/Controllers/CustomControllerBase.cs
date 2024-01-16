using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAPI.ExtensionMethods;

namespace WebAPI.Controllers
{
    public abstract class CustomControllerBase<CreateDTO, ReadDTO, UpdateDTO> : ControllerBase
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        protected readonly IBLL<CreateDTO, ReadDTO, UpdateDTO> _BLL;
        private readonly ILogger _logger;

        protected CustomControllerBase(IBLL<CreateDTO, ReadDTO, UpdateDTO> BLL, ILogger logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        public abstract Task<ActionResult<PagedListDTO<ReadDTO>>> GetPaged(PagerDTO pagerDTO);

        protected async Task<ActionResult<ReadDTO>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _BLL.GetByIdAsync(id);
                if (response.ValidateResponse.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }
        }

        protected async Task<ActionResult> CreateAsync(CreateDTO createDTO)
        {
            try
            {
                ResultResponseDTO<ReadDTO> response = await _BLL.InsertAsync(createDTO);
                if (response.ValidateResponse.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }

        }

        protected async Task<ActionResult> UpdateAsync(UpdateDTO updateDTO)
        {
            try
            {
                ResultResponseDTO<ReadDTO> response = await _BLL.UpdateAsync(updateDTO);
                if (response.ValidateResponse.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }
        }

        protected async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                IValidate response = await _BLL.DeleteAsync(id);
                if (response.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }
        }


    }
}
