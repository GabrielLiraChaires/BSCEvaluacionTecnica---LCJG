using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class ProductoDTO
    {
        [Required(ErrorMessage = "La clave del producto es obligatoria.")]
        [StringLength(100, ErrorMessage = "La clave no puede exceder de los 50 caracteres.")]
        public string? Clave { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "LEl nombre no puede exceder de los 100 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El número de existencias es obligatorio.")]
        public int Existencias { get; set; }

        [Required(ErrorMessage = "El costo de cada unidad es obligatorio.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostoUnidad { get; set; }
    }
}
