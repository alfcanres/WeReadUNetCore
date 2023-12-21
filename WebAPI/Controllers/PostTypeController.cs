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

        public PostTypeController(IPostTypeBLL BLL) : base(BLL)
        {
            _BLL = BLL;
        }

        [HttpGet("Paged")]
        public override async Task<ActionResult<PagedListDTO<PostTypeReadDTO>>> GetPaged([FromQuery] IPagerDTO pagerDTO)
        {
            var totalRecordsTask = _BLL.CountAllAsync();

            var listTask = _BLL.GetAllPagedAsync(pagerDTO);

            int totalRecords = await totalRecordsTask;
            var list = await listTask;

            PagedListDTO<PostTypeReadDTO> result = new PagedListDTO<PostTypeReadDTO>(list, totalRecords, pagerDTO);

            return Ok(result);

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

        [HttpGet("Top/{id}")]
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
