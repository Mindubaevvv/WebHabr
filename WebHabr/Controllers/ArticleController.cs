using Microsoft.AspNetCore.Mvc;

namespace WebHabr.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
