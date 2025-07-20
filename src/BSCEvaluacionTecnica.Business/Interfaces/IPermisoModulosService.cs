using BSCEvaluacionTecnica.Shared.DTOs;

namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IPermisoModulosService
    {
        Task<List<PermisoModuloUsuarioDTO>> ConsultarModulosUsuario(int id);
    }
}
