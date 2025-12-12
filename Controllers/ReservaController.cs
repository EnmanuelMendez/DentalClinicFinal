using DentalClinic.Data;
using DentalClinic.Models;
using DentalClinic.Models.ViewModels;
using DentalClinic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class ReservaController : Controller
    {
        private readonly DentalClinicContext _context;
        private readonly IServicioEmail _email;
        public ReservaController(DentalClinicContext context, IServicioEmail email)
        {
            _context = context;
            _email = email;
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

            var cliente = await _context.Clientes.FindAsync(vm.ClienteId);
            if (cliente == null) return BadRequest("Cliente no encontrado.");

            var reserva = new Reserva
            {
                ClienteId = vm.ClienteId,
                ServicioId = vm.ServicioId,
                DentistaId = vm.DentistaId,
                FechaHora = vm.FechaHora,
                PrecioTotal = servicio.CostoBase,
                Estado = "Pendiente"
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 💌  ENVIAR CORREO DE CONFIRMACIÓN AUTOMÁTICO
            try
            {
                Console.WriteLine(">>> [ReservaController] Entrando a envío de correo...");

                string asunto = $"Confirmación de Reserva - {servicio.Nombre}";

                string cuerpo = $@"
            <table style=""max-width:600px;font-family:Arial;margin:auto;"">
            <tr><td>

            <h2 style=""color:#0b5ed7;"">Reserva Confirmada</h2>

            <p>Hola <strong>{cliente.Nombre}</strong>,</p>

            <p>Tu reserva ha sido creada exitosamente.</p>

            <table style=""width:100%;border-collapse:collapse;margin-top:15px;"">
            <tr>
                <td style=""padding:8px;border:1px solid #ccc;"">Servicio:</td>
                <td style=""padding:8px;border:1px solid #ccc;"">{servicio.Nombre}</td>
            </tr>
            <tr>
                <td style=""padding:8px;border:1px solid #ccc;"">Fecha:</td>
                <td style=""padding:8px;border:1px solid #ccc;"">{reserva.FechaHora:dd/MM/yyyy HH:mm}</td>
            </tr>
            <tr>
                <td style=""padding:8px;border:1px solid #ccc;"">Precio:</td>
                <td style=""padding:8px;border:1px solid #ccc;"">{reserva.PrecioTotal:C}</td>
            </tr>
            </table>

            <p style=""margin-top:20px;"">Gracias por confiar en nosotros.</p>

            </td></tr>
            </table>
        ";

                await _email.EnviarEmail(cliente.Email!, asunto, cuerpo);

                Console.WriteLine(">>> [ReservaController] Correo enviado (o al menos intentado).");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> [ReservaController] ERROR ENVIANDO CORREO: " + ex.Message);
                Console.WriteLine(ex.ToString());
                // No rompas la experiencia del usuario si falla el correo
            }

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
