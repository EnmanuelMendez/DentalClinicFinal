using DentalClinic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class AdminReservaController : Controller
    {
        private readonly DentalClinicContext _context;

        public AdminReservaController(DentalClinicContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalReservas = _context.Reservas.Count();
            ViewBag.ReservasPendientes = _context.Reservas.Count(r => r.Estado == "Pendiente");
            ViewBag.ReservasConfirmadas = _context.Reservas.Count(r => r.Estado == "Confirmada");
            ViewBag.ReservasFinalizado = _context.Reservas.Count(r => r.Estado == "Finalizado");

            ViewBag.TotalClientes = _context.Clientes.Count();
            ViewBag.ClientesAnonimos = _context.Clientes.Count(c => c.Seguro == null);

            ViewBag.TotalServicios = _context.Servicios.Count();

            ViewBag.PromedioValoraciones = _context.Valoraciones.Any()
                ? _context.Valoraciones.Average(v => v.Puntuacion)
                : 0;

            ViewBag.TotalValoraciones = _context.Valoraciones.Count();
            ViewBag.TotalQuejas = _context.Quejas.Count();

            return View();
        }
    }
}
