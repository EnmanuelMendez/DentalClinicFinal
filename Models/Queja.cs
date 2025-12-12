using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Queja
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [Required, StringLength(200)]
        public string Asunto { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? Estado { get; set; }

        public string? Respuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }

        public Reserva? Reserva { get; set; }
    }
}
