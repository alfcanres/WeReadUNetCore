using BusinessLogicLayer.Interfaces;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public abstract class BaseController<CreateDTO, ReadDTO, UpdateDTO> : ControllerBase
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        protected readonly IBLL<CreateDTO, ReadDTO, UpdateDTO> _BLL;
        private readonly ILogger _logger;

        protected BaseController(IBLL<CreateDTO, ReadDTO, UpdateDTO> BLL, ILogger logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        protected async Task<ActionResult> GetByIdAsync(int id)
        {

            var response = await _BLL.GetByIdAsync(id);
            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }

        }

        protected async Task<ActionResult> CreateAsync(CreateDTO createDTO)
        {

            var response = await _BLL.InsertAsync(createDTO);
            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }


        }

        protected async Task<ActionResult> UpdateAsync(int id, UpdateDTO updateDTO)
        {

            var response = await _BLL.UpdateAsync(id, updateDTO);
            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        protected async Task<ActionResult> DeleteAsync(int id)
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


    }
}
