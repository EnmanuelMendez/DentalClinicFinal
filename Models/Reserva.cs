using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ServicioId { get; set; }

        [Required]
        public int DentistaId { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required, StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        [Required]
        public decimal PrecioTotal { get; set; }

        public string? AnonymousToken { get; set; }  // <-- NECESARIO

        public int? DuracionRealMinutos { get; set; }

        public Cliente? Cliente { get; set; }
        public Servicio? Servicio { get; set; }
        public Dentista? Dentista { get; set; }

        public ICollection<Queja>? Quejas { get; set; }
        public ICollection<Valoracion>? Valoraciones { get; set; }
        public ICollection<RegistroCorreo>? Correos { get; set; }
    }
}
