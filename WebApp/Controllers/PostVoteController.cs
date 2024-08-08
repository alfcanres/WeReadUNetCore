using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PostVoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
