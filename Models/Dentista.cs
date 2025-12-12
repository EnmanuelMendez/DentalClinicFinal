using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Dentista
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Telefono { get; set; }

        public int? ConsultorioId { get; set; }
        public Consultorio? Consultorio { get; set; }

        [StringLength(100)]
        public string? Especialidad { get; set; }

        // ================================
        // RELACIONES
        // ================================

        // Un dentista puede tener muchas reservas
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

        // Un dentista puede tener muchos horarios
        public ICollection<HorarioAtencion> Horarios { get; set; } = new List<HorarioAtencion>();
    }
}
