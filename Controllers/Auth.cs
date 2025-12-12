using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DentalClinic.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public IActionResult Login()
        {
            // Verifica sesión, no Identity
            if (HttpContext.Session.GetString("AdminLogged") == "true")
                return RedirectToAction("Dashboard", "Admin");

            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string usuario, string contraseña)
        {
            if (usuario == "admin" && contraseña == "admin")
            {
                HttpContext.Session.SetString("AdminLogged", "true");
                HttpContext.Session.SetString("AdminUser", usuario);

                return RedirectToAction("Dashboard", "AdminReserva");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
