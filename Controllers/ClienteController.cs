using DentalClinic.Data;
using DentalClinic.Helpers;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    [AdminRequired]
    public class ClienteController : Controller
    {
        private readonly DentalClinicContext _context;

        public ClienteController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Reservas)
                .ToListAsync();

            return View(clientes);
        }

        // GET: Crear
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear
        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Editar
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Editar
        [HttpPost]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Reservas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Confirmar eliminación
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Reservas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // AJAX → Edición en línea
        [HttpPost]
        public async Task<IActionResult> EditarCliente(int id, string nombre, string email, string telefono)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(nombre))
                cliente.Nombre = nombre;

            if (!string.IsNullOrWhiteSpace(email))
                cliente.Email = email;

            if (!string.IsNullOrWhiteSpace(telefono))
                cliente.Telefono = telefono;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cliente actualizado correctamente" });
        }
    }
}
