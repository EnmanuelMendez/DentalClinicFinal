using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Required]
        public int DuracionMinutos { get; set; } // <---

        public int MargenErrorMinutos { get; set; } = 0; // <---

        [Required]
        public decimal CostoBase { get; set; }

        public bool Activo { get; set; } = true; // <---

        public ICollection<ItemServicio>? Items { get; set; }
        public ICollection<Reserva>? Reservas { get; set; }
    }
}
