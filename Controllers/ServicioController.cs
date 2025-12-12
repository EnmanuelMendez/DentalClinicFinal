using DentalClinic.Data;
using DentalClinic.Helpers;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    [AdminRequired]
    public class ServicioController : Controller
    {
        private readonly DentalClinicContext _context;

        public ServicioController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Servicios.ToListAsync();
            return View(lista);
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            return servicio == null ? NotFound() : View(servicio);
        }

        // CREAR
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (!ModelState.IsValid)
                return View(servicio);

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDITAR
        public async Task<IActionResult> Edit(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            return servicio == null ? NotFound() : View(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Servicio servicio)
        {
            if (!ModelState.IsValid)
                return View(servicio);

            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR
        public async Task<IActionResult> Delete(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            return servicio == null ? NotFound() : View(servicio);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
