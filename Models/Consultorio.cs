using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Consultorio
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Pasillo { get; set; }

        [StringLength(50)]
        public string? NumeroPuerta { get; set; }

        // Navegación inversa
        public ICollection<Dentista>? Dentistas { get; set; }
    }
}
