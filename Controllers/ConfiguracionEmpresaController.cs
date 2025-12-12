using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Controllers
{
    public class ConfiguracionEmpresaController : Controller
    {
        private readonly DentalClinicContext _context;

        public ConfiguracionEmpresaController(DentalClinicContext context)
        {
            _context = context;
        }

        // LISTAR (normalmente solo habrá 1 registro)
        public async Task<IActionResult> Index()
        {
            var data = await _context.ConfiguracionesEmpresa.ToListAsync();
            return View(data);
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var config = await _context.ConfiguracionesEmpresa.FindAsync(id);
            return config == null ? NotFound() : View(config);
        }

        // CREAR
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConfiguracionEmpresa config)
        {
            if (!ModelState.IsValid)
                return View(config);

            _context.ConfiguracionesEmpresa.Add(config);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDITAR
        public async Task<IActionResult> Edit(int id)
        {
            var config = await _context.ConfiguracionesEmpresa.FindAsync(id);
            return config == null ? NotFound() : View(config);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConfiguracionEmpresa config)
        {
            if (!ModelState.IsValid)
                return View(config);

            _context.ConfiguracionesEmpresa.Update(config);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ELIMINAR
        public async Task<IActionResult> Delete(int id)
        {
            var config = await _context.ConfiguracionesEmpresa.FindAsync(id);
            return config == null ? NotFound() : View(config);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var config = await _context.ConfiguracionesEmpresa.FindAsync(id);

            if (config != null)
            {
                _context.ConfiguracionesEmpresa.Remove(config);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
