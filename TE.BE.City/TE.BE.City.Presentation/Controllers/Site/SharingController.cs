using Microsoft.AspNetCore.Mvc;

namespace TE.BE.City.Presentation.Controllers
{
    [Route("site/[controller]")]
    public class SharingController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
