using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSCEvaluacionTecnica.Shared.DTOs;

namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IAccesoService
    {
        Task<SesionDTO> Login(LoginDTO loginDTO);
        Task<string> ValidarToken(string token);
    }
}
