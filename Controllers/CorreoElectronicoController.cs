using DentalClinic.Services;
using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorreoElectronicoController : ControllerBase
    {
        private readonly IServicioEmail _email;

        public CorreoElectronicoController(IServicioEmail email)
        {
            _email = email;
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> Enviar([FromBody] EmailRequest request)
        {
            await _email.EnviarEmail(request.Email, request.Tema, request.Cuerpo);
            return Ok("Correo enviado correctamente.");
        }
    }
}
