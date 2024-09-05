using Microsoft.AspNetCore.Mvc;

namespace Web_253502_Yarashuk.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/submit")]
        public IActionResult SubmitForm(string login, string password)
        {
            // Логика обработки данных формы
            return View();
        }

    }
}
