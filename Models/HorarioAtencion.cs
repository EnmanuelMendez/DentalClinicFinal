using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class HorarioAtencion
    {
        [Key]
        public int Id { get; set; }

        [Required, Range(1, 7)]
        public int DiaSemana { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

        [Required]
        public int DentistaId { get; set; }
        public Dentista? Dentista { get; set; }
    }
}
