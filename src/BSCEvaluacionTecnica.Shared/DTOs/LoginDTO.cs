
using System.ComponentModel.DataAnnotations;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El campo correo electronico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo correo electronico no puede exceder de 100 caracteres.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo clave es obligatorio.")]
        public string Clave { get; set; }
    }
}
