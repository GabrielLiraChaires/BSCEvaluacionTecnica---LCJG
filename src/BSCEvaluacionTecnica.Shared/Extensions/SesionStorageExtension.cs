using Blazored.SessionStorage;
using System.Text.Json;

namespace BSCEvaluacionTecnica.Shared.Extensions
{
    public static class SesionStorageExtension
    {
        //Función para guardar en storage la sesión.
        public static async Task GuardarStorage<T>
           (this ISessionStorageService sessionStorageService,
           string key, T item) where T : class
        {
            //Serializar a formato Json la clase enviada con usuario y clave.
            var itemJson = JsonSerializer.Serialize(item);
            //Guardar esta clase con la llave enviada.
            await sessionStorageService.SetItemAsStringAsync(key, itemJson);

        }

        //Función para obtener la sesión en storage.
        public static async Task<T?> ObtenerStorage<T>
            (this ISessionStorageService sessionStorageService,
            string key) where T : class
        {
            //Obtener la clase guardada con la llave.
            var itemJson = await sessionStorageService.GetItemAsStringAsync(key);
            //Buscar itemJson y retornarlo en caso de que exista.
            if (itemJson != null)
            {
                var item = JsonSerializer.Deserialize<T>(itemJson);
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}
