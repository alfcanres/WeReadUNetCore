using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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

        public async Task<IActionResult> Index(PagerDTO pagerDTO = null)
        {
            string token = Convert.ToString(HttpContext.Request.Cookies["AuthToken"]);

            _repository.SetBearerToken(token);

            if (pagerDTO == null)
            {
                pagerDTO = new PagerDTO() { CurrentPage = 1, RecordsPerPage = 10, SearchKeyWord = "" };
            }


            var response = await _repository.GetPagedAsync(pagerDTO);

            return View(response);
        }



    }
}
