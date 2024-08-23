using DataTransferObjects.DTO;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.Post;
using WebAPI.Client.ViewModels;
using WebApp.Models;
using WebAPI.Client.Repository.PostType;
using WebAPI.Client.Repository.MoodType;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAPI.Client.Repository.ApplicationUserInfo;



namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        private readonly ILogger<PostController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostTypeRepository _postTypeRepository;
        private readonly IMoodTypeRepository _moodTypeRepository;
        private readonly IApplicationUserInfoRepository _applicationUserInfoRepository;
        private readonly string _authToken;
        private readonly int _currentUserID;

        public PostController(
            IPostRepository repository,
            IPostTypeRepository postTypeRepository,
            IMoodTypeRepository moodTypeRepository,
            IApplicationUserInfoRepository applicationUserInfoRepository,
            ILogger<PostController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _postTypeRepository = postTypeRepository;
            _moodTypeRepository = moodTypeRepository;
            _applicationUserInfoRepository = applicationUserInfoRepository;
            _authToken = Convert.ToString(_httpContextAccessor.HttpContext.Request.Cookies["AuthToken"]);

            if (!string.IsNullOrEmpty(_authToken))
            {
                _currentUserID = Helpers.ClaimsHelper.GetApplicationUserInfoId(_authToken);

                _repository.SetBearerToken(_authToken);
            }

        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {

            var response = await _repository.GetPublishedPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public async Task<IActionResult> MyPosts(PagerParams pager = null)
        {


            var response = await _repository.GetAllPagedByUserAsync(_currentUserID, pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            var responseStatus = await LoadDropDowns();

            if(responseStatus == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(new PostCreateDTO()
            {
                ApplicationUserInfoId = _currentUserID
            }
            );
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDTO createDTO)
        {
          

            if (!ModelState.IsValid)
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(createDTO);
            }

            var response = await _repository.CreateAsync(createDTO);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("MyPosts", "Post");
            }
            else
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }


                return View(createDTO);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {

            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if(response.Content.ApplicationUserInfoId != _currentUserID)
            {
                return RedirectToAction("MyPosts", "Post");
            }

            var responseStatus = await LoadDropDowns();

            if (responseStatus == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            PostUpdateDTO PostUpdateDTO = new PostUpdateDTO
            {
                Id = response.Content.Id,
                ApplicationUserInfoId = response.Content.ApplicationUserInfoId,
                MoodTypeId = response.Content.MoodTypeId,
                PostTypeId = response.Content.PostTypeId,
                Title = response.Content.Title,
                Text = response.Content.Text,

            };


            return View(PostUpdateDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PostUpdateDTO editModel)
        {


            if (!ModelState.IsValid)
            {
                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(editModel);
            }

            var response = await _repository.UpdateAsync(editModel);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                var responseStatus = await LoadDropDowns();

                if (responseStatus == ResponseStatus.Unauthorized)
                {
                    return RedirectToAction("Logout", "Account");
                }

                return View(editModel);
            }
        }

        public async Task<IActionResult> View(int id)
        {



            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            return View(response.Content);
        }


        public async Task<IActionResult> Delete(int id)
        {


            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response.Content);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(PostUpdateDTO editModel)
        {


            var response = await _repository.DeleteAsync(editModel.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(editModel);
            }
        }

        private async Task<ResponseStatus> LoadDropDowns()
        {
            var types = await _postTypeRepository.GetAllByIsAvailableAsync(true);
            var moods = await _moodTypeRepository.GetIsAvailableAsync(true);

            if (moods.Status != ResponseStatus.Unauthorized && types.Status != ResponseStatus.Unauthorized)
            {
                ViewBag.MoodTypes = new SelectList(moods.Content.List, "Id", "Mood");
                ViewBag.PostTypes = new SelectList(types.Content.List, "Id", "Description");
            }

            return moods.Status == ResponseStatus.Unauthorized || types.Status == ResponseStatus.Unauthorized ? ResponseStatus.Unauthorized : ResponseStatus.Success;

        }
    }
}
