using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(11)]
        public string? Cedula { get; set; }   // <-- NECESARIO PARA EL CONTROLADOR

        public bool IsAnonymous { get; set; } = false; // <-- usado en PublicReserva

        [StringLength(50)]
        public string? Seguro { get; set; }

        public DateTime? UltimaReserva { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
    }
}
