using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class DentistaController : Controller
    {
        private readonly DentalClinicContext _context;

        public DentistaController(DentalClinicContext context)
        {
            _context = context;
        }

        // ==========================
        // LISTA GENERAL
        // ==========================
        public async Task<IActionResult> Index()
        {
            var dentistas = await _context.Dentistas
                .Include(d => d.Consultorio)
                .ToListAsync();

            return View(dentistas);
        }

        // ==========================
        // DETALLES
        // ==========================
        public async Task<IActionResult> Details(int id)
        {
            var dentista = await _context.Dentistas
                .Include(d => d.Consultorio)
                .FirstOrDefaultAsync(d => d.Id == id);

            return dentista == null ? NotFound() : View(dentista);
        }

        // ==========================
        // CREAR
        // ==========================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dentista dentista)
        {
            if (!ModelState.IsValid)
                return View(dentista);

            _context.Dentistas.Add(dentista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // EDITAR
        // ==========================
        public async Task<IActionResult> Edit(int id)
        {
            var dentista = await _context.Dentistas.FindAsync(id);
            return dentista == null ? NotFound() : View(dentista);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Dentista dentista)
        {
            if (!ModelState.IsValid)
                return View(dentista);

            _context.Dentistas.Update(dentista);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // ELIMINAR
        // ==========================
        public async Task<IActionResult> Delete(int id)
        {
            var dentista = await _context.Dentistas.FindAsync(id);
            return dentista == null ? NotFound() : View(dentista);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dentista = await _context.Dentistas.FindAsync(id);

            if (dentista != null)
            {
                _context.Dentistas.Remove(dentista);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
