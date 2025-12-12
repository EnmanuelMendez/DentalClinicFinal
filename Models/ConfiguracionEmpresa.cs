using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class ConfiguracionEmpresa
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Telefono { get; set; }

        [StringLength(500)]
        public string? HorariosDescripcion { get; set; }
    }
}
