using Microsoft.AspNetCore.Mvc;
using Plugin1.Areas.Plugin1.ViewModels;

namespace Plugin1.Areas.Plugin1.Controllers
{
    [Area("Plugin1")]
    public class Plugin1HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new MyViewModel
            {
                Id = 1,
                Name = "My View Model"
            };

            return Ok(model);
        }
    }
}
