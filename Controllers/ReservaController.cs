using DentalClinic.Data;
using DentalClinic.Models;
using DentalClinic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class ReservaController : Controller
    {
        private readonly DentalClinicContext _context;

        public ReservaController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTADO GENERAL
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .OrderByDescending(r => r.FechaHora)
                .ToListAsync();

            return View(reservas);
        }

        // BUSCAR POR EMAIL
        public IActionResult Buscar() => View();

        [HttpPost]
        public async Task<IActionResult> Buscar(string email)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

            if (cliente == null)
            {
                ViewBag.Error = "No existe un cliente con ese correo electrónico.";
                return View();
            }

            return RedirectToAction("MisReservas", new { clienteId = cliente.Id });
        }

        // LISTADO POR CLIENTE
        public async Task<IActionResult> MisReservas(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null) return NotFound();

            var reservas = await _context.Reservas
                .Include(r => r.Servicio)
                .Where(r => r.ClienteId == clienteId)
                .ToListAsync();

            ViewBag.ClienteNombre = cliente.Nombre;

            return View(reservas);
        }

        // CREAR
        public async Task<IActionResult> Create(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null) return NotFound();

            var vm = new ReservaViewModel
            {
                Cliente = cliente,
                ClienteId = cliente.Id,  // ← Añadir esto
                Servicios = new SelectList(_context.Servicios, "Id", "Nombre"),
                Dentistas = new SelectList(_context.Dentistas, "Id", "Nombre")
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
                return View(vm);
            }

            var servicio = await _context.Servicios.FindAsync(vm.ServicioId);
            if (servicio == null) return BadRequest();

            var reserva = new Reserva
            {
                ClienteId = vm.ClienteId,   // ← NO usar vm.Cliente.Id
                ServicioId = vm.ServicioId,
                DentistaId = vm.DentistaId,
                FechaHora = vm.FechaHora,
                PrecioTotal = servicio.CostoBase,
                Estado = "Pendiente"
            };


            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction("MisReservas", new { clienteId = reserva.ClienteId });
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserva == null ? NotFound() : View(reserva);
        }

        // EDITAR
        public async Task<IActionResult> Edit(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null) return NotFound();

            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");

            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
                return View(reserva);
            }

            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction("MisReservas", new { clienteId = reserva.ClienteId });
        }

        // CANCELAR
        public async Task<IActionResult> Cancelar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            return RedirectToAction("MisReservas", new { clienteId = reserva.ClienteId });
        }
    }
}
