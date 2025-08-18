using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyMS.Controllers
{
    public class AuthViewController: Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session/token
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}