using System.ComponentModel.DataAnnotations;

namespace DentalClinic.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Tipo { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
