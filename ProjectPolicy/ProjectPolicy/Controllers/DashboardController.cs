using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

