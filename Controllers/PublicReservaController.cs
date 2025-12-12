using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;

namespace DentalClinic.Controllers
{
    public class PublicReservaController : Controller
    {
        private readonly DentalClinicContext _context;
        private readonly IConfiguration _config;

        public PublicReservaController(DentalClinicContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // =========================================
        // GET: Crear Reserva Pública
        // =========================================
        public IActionResult Create()
        {
            ViewBag.Servicios = _context.Servicios
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Nombre} - ${s.CostoBase:F2}"
                })
                .ToList();

            ViewBag.Dentistas = _context.Dentistas
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Nombre
                })
                .ToList();

            return View();
        }

        // =========================================
        // POST: Crear Reserva Pública
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create(
            string nombre,
            string email,
            string telefono,
            string cedula,
            int servicioId,
            int dentistaId,
            int dia,
            string hora)
        {
            // ===== VALIDACIONES =====
            if (string.IsNullOrWhiteSpace(nombre))
            {
                ViewBag.Error = "El nombre completo es obligatorio.";
                RecargarCombos();
                return View();
            }

            if (string.IsNullOrWhiteSpace(cedula))
            {
                ViewBag.Error = "La cédula es obligatoria.";
                RecargarCombos();
                return View();
            }

            if (string.IsNullOrWhiteSpace(email) || servicioId <= 0)
            {
                ViewBag.Error = "Email y servicio son requeridos.";
                RecargarCombos();
                return View();
            }

            if (dentistaId <= 0)
            {
                ViewBag.Error = "Debe seleccionar un dentista.";
                RecargarCombos();
                return View();
            }

            if (dia == 0 || string.IsNullOrWhiteSpace(hora))
            {
                ViewBag.Error = "Debe seleccionar un día y hora disponible.";
                RecargarCombos();
                return View();
            }

            // ===== CREAR CLIENTE ANÓNIMO =====
            var cliente = new Cliente
            {
                Nombre = nombre.Trim(),
                Cedula = cedula.Trim(),
                Email = email.Trim(),
                Telefono = telefono ?? string.Empty,
                IsAnonymous = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var servicio = await _context.Servicios.FindAsync(servicioId);
            if (servicio == null) return BadRequest();

            // ===== CONSTRUIR FECHA COMPLETA =====
            DateTime fecha = ObtenerProximaFecha(dia)
                                .Add(TimeSpan.Parse(hora));

            // Validar si la hora ya está ocupada
            bool ocupada = _context.Reservas
                .Any(r => r.DentistaId == dentistaId &&
                          r.FechaHora == fecha);

            if (ocupada)
            {
                ViewBag.Error = "Esta hora ya fue reservada. Selecciona otra.";
                RecargarCombos();
                return View();
            }

            // ===== CREAR RESERVA =====
            var reserva = new Reserva
            {
                ClienteId = cliente.Id,
                ServicioId = servicioId,
                DentistaId = dentistaId,
                FechaHora = fecha,
                PrecioTotal = servicio.CostoBase,
                Estado = "Pendiente",
                AnonymousToken = GenerateToken()
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // ===== ENVIAR CORREO =====
            var urlBase = $"{Request.Scheme}://{Request.Host}";
            var link = $"{urlBase}/PublicReserva/ViewReservation?token={reserva.AnonymousToken}";

            await SendEmailAsync(email, "Su reserva - DentalClinic",
                $"Gracias por su reserva.\nPuede ver el estado aquí:\n{link}");

            return View("Thanks");
        }

        private void RecargarCombos()
        {
            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
            ViewBag.Dentistas = new SelectList(_context.Dentistas, "Id", "Nombre");
        }

        // =========================================
        // API — Días disponibles según dentista
        // =========================================
        public IActionResult ObtenerDiasDisponibles(int dentistaId)
        {
            var horarios = _context.HorariosAtencion
                .Where(h => h.DentistaId == dentistaId)
                .ToList();

            var dias = horarios
                .Select(h => new
                {
                    value = h.DiaSemana,
                    text = ObtenerNombreDia(h.DiaSemana)
                })
                .Distinct();

            return Json(dias);
        }

        private string ObtenerNombreDia(int n) =>
            n switch
            {
                1 => "Lunes",
                2 => "Martes",
                3 => "Miércoles",
                4 => "Jueves",
                5 => "Viernes",
                6 => "Sábado",
                7 => "Domingo",
                _ => "Desconocido"
            };

        // =========================================
        // API — Horas disponibles según día
        // =========================================
        public IActionResult ObtenerHorasDisponibles(int dentistaId, int dia)
        {
            var horarios = _context.HorariosAtencion
                .Where(h => h.DentistaId == dentistaId && h.DiaSemana == dia)
                .ToList();

            var horasOcupadas = _context.Reservas
                .Where(r => r.DentistaId == dentistaId &&
                            (int)r.FechaHora.DayOfWeek == (dia - 1))
                .Select(r => r.FechaHora.TimeOfDay)
                .ToList();

            List<string> horasDisponibles = new();

            foreach (var h in horarios)
            {
                var hora = h.HoraInicio;
                while (hora < h.HoraFin)
                {
                    if (!horasOcupadas.Contains(hora))
                        horasDisponibles.Add(hora.ToString(@"hh\:mm"));

                    hora = hora.Add(TimeSpan.FromMinutes(30)); // intervalo
                }
            }

            return Json(horasDisponibles);
        }

        private DateTime ObtenerProximaFecha(int diaSemana)
        {
            var hoy = DateTime.Today;
            int diff = diaSemana - (int)hoy.DayOfWeek;
            if (diff <= 0) diff += 7;
            return hoy.AddDays(diff);
        }

        // =========================================
        // VER RESERVAS por TOKEN
        // =========================================
        public async Task<IActionResult> ViewReservation(string token)
        {
            if (string.IsNullOrEmpty(token))
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.AnonymousToken == token);

            if (reserva == null)
                return NotFound();

            var reservasCliente = await _context.Reservas
                .Include(r => r.Servicio)
                .Where(r => r.ClienteId == reserva.ClienteId)
                .OrderByDescending(r => r.FechaHora)
                .ToListAsync();

            ViewBag.Token = token;

            return View(reservasCliente);
        }

        // =========================================
        // QUEJA
        // =========================================
        public async Task<IActionResult> Queja(string token, int reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.Id == reservaId &&
                                          r.AnonymousToken == token);

            return reserva == null ? NotFound() : View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Queja(int reservaId, string asunto, string descripcion)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva == null) return NotFound();

            var q = new Queja
            {
                ReservaId = reservaId,
                Asunto = asunto,
                Descripcion = descripcion
            };

            _context.Quejas.Add(q);
            await _context.SaveChangesAsync();

            return View("GraciasQueja");
        }

        // =========================================
        // VALORACIÓN
        // =========================================
        public async Task<IActionResult> Valoracion(string token, int reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.Id == reservaId &&
                                          r.AnonymousToken == token);

            return reserva == null ? NotFound() : View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Valoracion(int reservaId, int estrellas, string comentario)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva == null) return NotFound();

            var val = new Valoracion
            {
                ReservaId = reservaId,
                Puntuacion = Math.Clamp(estrellas, 1, 5),
                Comentario = comentario
            };

            _context.Valoraciones.Add(val);
            await _context.SaveChangesAsync();

            return View("GraciasValoracion");
        }

        // =========================================
        // HELPERS
        // =========================================
        private string GenerateToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes)
                .Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var host = _config["Smtp:Host"];
                var port = int.TryParse(_config["Smtp:Port"], out var p) ? p : 25;

                var mail = new MailMessage
                {
                    From = new MailAddress(_config["Smtp:From"] ?? "no-reply@dentalclinic.local"),
                    Subject = subject,
                    Body = body
                };

                mail.To.Add(toEmail);

                using var client = new SmtpClient(host, port)
                {
                    EnableSsl = true
                };

                await client.SendMailAsync(mail);
            }
            catch { }
        }
    }
}
