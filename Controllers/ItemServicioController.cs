using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class ItemServicioController : Controller
    {
        private readonly DentalClinicContext _context;

        public ItemServicioController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTAR ITEMS
        public async Task<IActionResult> Index()
        {
            var items = await _context.ItemsServicio
                .Include(i => i.Servicio)
                .OrderBy(i => i.Servicio!.Nombre)
                .ThenBy(i => i.Nombre)
                .ToListAsync();

            return View(items);
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.ItemsServicio
                .Include(i => i.Servicio)
                .FirstOrDefaultAsync(i => i.Id == id);

            return item == null ? NotFound() : View(item);
        }

        // CREAR
        public IActionResult Create()
        {
            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemServicio item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
                return View(item);
            }

            _context.ItemsServicio.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // EDITAR
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.ItemsServicio.FindAsync(id);
            if (item == null) return NotFound();

            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemServicio item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
                return View(item);
            }

            _context.ItemsServicio.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ItemsServicio
                .Include(i => i.Servicio)
                .FirstOrDefaultAsync(i => i.Id == id);

            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ItemsServicio.FindAsync(id);
            if (item != null)
            {
                _context.ItemsServicio.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
