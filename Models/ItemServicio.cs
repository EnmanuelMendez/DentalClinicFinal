using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Models
{
    public class ItemServicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Servicio")]
        public int ServicioId { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal CostoAdicional { get; set; }

        public Servicio? Servicio { get; set; }
    }
}
