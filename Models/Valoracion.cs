using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Valoracion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [Required]
        public int ServicioId { get; set; } // <-- necesario para el controlador

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; } // <-- antes Estrellas

        [StringLength(300)]
        public string? Comentario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public Reserva? Reserva { get; set; }
    }
}
