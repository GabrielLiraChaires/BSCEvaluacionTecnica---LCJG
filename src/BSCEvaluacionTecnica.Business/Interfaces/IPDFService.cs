using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSCEvaluacionTecnica.Shared.DTOs;
namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IPDFService
    {
        Task<string> PDF(PDF parametros);
    }
}
