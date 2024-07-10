using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Client.Repository.MoodType;

namespace WebApp.Controllers
{
    [Authorize]
    public class MoodTypeController : Controller
    {

        private readonly IMoodTypeRepository _repository;
        private readonly ILogger<MoodTypeController> _logger;

        public MoodTypeController(IMoodTypeRepository repository, ILogger<MoodTypeController> logger)
        {
            _repository = repository;
            _logger = logger;


        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {
            string token = Convert.ToString(HttpContext.Request.Cookies["AuthToken"]);

            _repository.SetBearerToken(token);

            if (pager == null)
            {
                pager = new PagerParams() { CurrentPage = 1, RecordsPerPage = 10, SearchKeyWord = "" };
            }


            var response = await _repository.GetPagedAsync(pager);

            return View(response);
        }

        public IActionResult Create()
        {
            return View(new MoodTypeCreateDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create(MoodTypeCreateDTO createModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createModel);
            }

            string token = Convert.ToString(HttpContext.Request.Cookies["AuthToken"]);

            _repository.SetBearerToken(token);

            var response = await _repository.CreateAsync(createModel);

            if (response.Validate.IsValid)
            {
                return RedirectToAction("Index", "MoodType");
            }
            else
            {
                foreach (var error in response.Validate.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(createModel);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            string token = Convert.ToString(HttpContext.Request.Cookies["AuthToken"]);

            _repository.SetBearerToken(token);

            var response = await _repository.GetByIdAsync(id);

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MoodTypeUpdateDTO editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            string token = Convert.ToString(HttpContext.Request.Cookies["AuthToken"]);

            _repository.SetBearerToken(token);

            var response = await _repository.UpdateAsync(editModel);

            if (response.Validate.IsValid)
            {
                return RedirectToAction("Index", "MoodType");
            }
            else
            {
                foreach (var error in response.Validate.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(editModel);
            }
        }



    }
}
