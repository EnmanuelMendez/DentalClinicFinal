using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class RegistroCorreo
    {
        [Key]
        public int Id { get; set; }

        public int? ReservaId { get; set; }

        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }

        public string? Estado { get; set; }
        public string? Error { get; set; }

        public Reserva? Reserva { get; set; }
    }
}
