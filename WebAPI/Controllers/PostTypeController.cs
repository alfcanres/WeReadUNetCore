using BusinessLogicLayer.BusinessObjects;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypeController : CustomControllerBase<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        private readonly IPostTypeBLL _BLL;
        private readonly ILogger<PostTypeController> _logger;

        public PostTypeController(IPostTypeBLL BLL, ILogger<PostTypeController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpGet("Paged")]
        public override async Task<ActionResult<PagedListDTO<PostTypeReadDTO>>> GetPaged([FromQuery] PagerDTO pagerDTO)
        {
            var totalRecords = await _BLL.CountAllAsync();
            var list = await _BLL.GetAllPagedAsync(pagerDTO);

            IValidate validate = _BLL.IsOperationValid();

            if (validate.IsValid && list != null)
            {
                PagedListDTO<PostTypeReadDTO> result = new PagedListDTO<PostTypeReadDTO>(list, totalRecords, pagerDTO);
                return Ok(result);
            }
            else
            {
                return StatusCode(500, validate);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostTypeReadDTO>> Get(int id)
        {
            return await GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostTypeCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostTypeUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

        [HttpGet("Top/{top}")]
        public async Task<ActionResult<IEnumerable<PostTypeReadDTO>>> GetTop(int top)
        {
            var result = await _BLL.GetTopWithPostsAsync(top);

            return Ok(result);
        }

        [HttpGet("Available/{isAvalable}")]
        public async Task<ActionResult<IEnumerable<PostTypeReadDTO>>> GetIsAvailable(bool isAvalable)
        {

            var result = await _BLL.GetAllByIsAvailableAsync(isAvalable);

            return Ok(result);
        }
    }
}
