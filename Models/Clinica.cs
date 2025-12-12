using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Clinica
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(20)]
        public string? RNC { get; set; }
    }
}
