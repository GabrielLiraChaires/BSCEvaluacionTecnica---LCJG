using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class UsuarioDTO : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder de los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$", ErrorMessage = "Solo se permiten letras y espacios en blanco.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder de los 100 caracteres.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El campo rol es obligatorio.")]
        [StringLength(23, ErrorMessage = "El campo rol no puede exceder de los 23 caracteres.")]
        [RegularExpression(@"^(Administrador|Personal Administrativo|Vendedor)$", ErrorMessage = "El campo rol debe ser válido.")]
        public string? Rol { get; set; }

        public string? Clave { get; set; }

        [Compare(nameof(Clave), ErrorMessage = "- Las claves no coinciden.")]
        public string? ClaveConfirmacion { get; set; }

        public bool Edicion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validaciones comunes si se provee Clave.
            if (!string.IsNullOrWhiteSpace(Clave))
            {
                if (Clave.Length < 8)
                    yield return new ValidationResult("- La clave debe tener al menos 8 caracteres.", new[] { nameof(Clave) });

                var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]+$");
                if (!regex.IsMatch(Clave))
                    yield return new ValidationResult("- La clave debe contener al menos una letra mayúscula, un número y no debe incluir caracteres especiales.", new[] { nameof(Clave) });

                if (Clave != ClaveConfirmacion)
                    yield return new ValidationResult("- Las claves no coinciden.", new[] { nameof(ClaveConfirmacion) });
            }

            // Validación de contraseña al crear (Edicion == false).
            if (!Edicion)
            {
                if (string.IsNullOrWhiteSpace(Clave))
                    yield return new ValidationResult("- La clave es obligatoria.", new[] { nameof(Clave) });

                if (string.IsNullOrWhiteSpace(ClaveConfirmacion))
                    yield return new ValidationResult("- La confirmación de la clave es obligatoria.", new[] { nameof(ClaveConfirmacion) });
            }
        }
    }
}
