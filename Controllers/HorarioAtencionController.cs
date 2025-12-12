using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class HorarioAtencionController : Controller
    {
        private readonly DentalClinicContext _context;

        public HorarioAtencionController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var lista = await _context.HorariosAtencion
                .Include(h => h.Dentista)
                .OrderBy(h => h.DiaSemana)
                .ThenBy(h => h.Dentista!.Nombre)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync();

            return View(lista);
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var horario = await _context.HorariosAtencion
                .Include(h => h.Dentista)
                .FirstOrDefaultAsync(h => h.Id == id);

            return horario == null ? NotFound() : View(horario);
        }

        // CREAR (GET)
        public IActionResult Create()
        {
            ViewBag.Dentistas = new SelectList(
                _context.Dentistas
                    .OrderBy(d => d.Nombre)
                    .ToList(),
                "Id",
                "Nombre"
            );

            return View();
        }

        // CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorarioAtencion horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Dentistas = new SelectList(
                    _context.Dentistas
                        .OrderBy(d => d.Nombre)
                        .ToList(),
                    "Id",
                    "Nombre",
                    horario.DentistaId
                );
                return View(horario);
            }

            _context.HorariosAtencion.Add(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var horario = await _context.HorariosAtencion.FindAsync(id);
            if (horario == null) return NotFound();

            ViewBag.Dentistas = new SelectList(
                _context.Dentistas
                    .OrderBy(d => d.Nombre)
                    .ToList(),
                "Id",
                "Nombre",
                horario.DentistaId
            );

            return View(horario);
        }

        // EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HorarioAtencion horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Dentistas = new SelectList(
                    _context.Dentistas
                        .OrderBy(d => d.Nombre)
                        .ToList(),
                    "Id",
                    "Nombre",
                    horario.DentistaId
                );
                return View(horario);
            }

            _context.HorariosAtencion.Update(horario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var horario = await _context.HorariosAtencion
                .Include(h => h.Dentista)
                .FirstOrDefaultAsync(h => h.Id == id);

            return horario == null ? NotFound() : View(horario);
        }

        // ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.HorariosAtencion.FindAsync(id);

            if (horario != null)
            {
                _context.HorariosAtencion.Remove(horario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
