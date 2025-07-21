using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSCEvaluacionTecnica.Shared.DTOs;
using System.Threading.Tasks;

namespace BSCEvaluacionTecnica.Business.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> Consultar();
        Task<ResponseAPI<ProductoDTO>> Guardar(ProductoDTO producto);
        Task<ResponseAPI<ProductoDTO>> Actualizar(ProductoDTO producto);
    }
}
