using BSCEvaluacionTecnica.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Consultar();
    }
}
