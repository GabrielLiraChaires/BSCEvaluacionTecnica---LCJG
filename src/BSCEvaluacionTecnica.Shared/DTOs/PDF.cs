using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class PDF
    {
        public string? Html { get; set; }
        public string? Orientacion { get; set; }
        public double[]? Margenes { get; set; }
    }
}
