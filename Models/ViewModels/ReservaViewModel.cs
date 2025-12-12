using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalClinic.Models.ViewModels
{
    public class ReservaViewModel
    {
        // Datos del cliente
        public Cliente? Cliente { get; set; }

        [Required]
        public int ClienteId { get; set; }

        // Selección del servicio
        [Required]
        public int ServicioId { get; set; }

        // Selección del dentista
        [Required]
        public int DentistaId { get; set; }

        // Fecha y hora de la cita
        [Required]
        public DateTime FechaHora { get; set; }

        public decimal PrecioTotal { get; set; }

        // Listas para los dropdowns
        public IEnumerable<SelectListItem>? Servicios { get; set; }
        public IEnumerable<SelectListItem>? Dentistas { get; set; }
    }
}
