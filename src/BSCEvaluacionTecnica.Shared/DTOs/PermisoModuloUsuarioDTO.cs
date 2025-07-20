
namespace BSCEvaluacionTecnica.Shared.DTOs
{
    public class PermisoModuloUsuarioDTO
    {
        public int FkIdUsuario { get; set; }
        public int FkIdModuloSistema { get; set; }
        public string? Acceso { get; set; }
        public string? ModuloSistema { get; set; }
        public string? Correo { get; set; }
        public string? NombreCompletoUsuario { get; set; }
        public string? RolUsuario { get; set; }
    }
}
