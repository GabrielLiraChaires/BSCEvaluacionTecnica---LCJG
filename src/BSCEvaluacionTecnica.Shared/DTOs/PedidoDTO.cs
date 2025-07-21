using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El cliente no puede exceder de 100 caracteres.")]
        public string? Cliente { get; set; } = string.Empty;

        public int FkIdVendedor { get; set; }

        public UsuarioDTO? Vendedor { get; set; }
    }
}
