# 🏢 Sistema de Administración de Negocios

Aplicación web en .NET para la gestión integral de la empresa BSC. Incluye:

- **Autenticación** con JSON Web Tokens (JWT, duración: 15 min).  
- **Frontend** en Blazor WebAssembly.  
- **Backend**: API RESTful en ASP .NET Core con C#.  
- **Acceso a datos**: Entity Framework Core y objetos de base de datos tradicionales (Vistas, Stored Procedures, Triggers).

---

## 🎬 Video demostrativo

[Ver demo en Google Drive](https://drive.google.com/file/d/1K7zk_3Q8YdOTdytEveC_Z9-4DR7h0jze/view?usp=sharing)

> **Nota:**  
> - En el script de base de datos (en la raíz del repositorio) encontrarás todas las Vistas, Stored Procedures y Triggers.  
> - Se usa Entity Framework Core en la mayoría de las operaciones para demostrar su integración con SQL Server, reduciendo código repetitivo y errores de SQL manual.  
> - La solución sigue una **arquitectura por capas**:  
>   - **Client** (UI en Blazor WASM)  
>   - **Business** (lógica de negocio)  
>   - **DataAccess** (acceso a datos)  
>   - **Server** (API REST)  
>   - **Shared** (DTOs y extensiones de autenticación)

---

## 🛠️ Entorno de desarrollo

| Componente         | Versión / Detalle             |
|--------------------|--------------------------------|
| **IDE**            | Visual Studio 2022             |
| **Lenguajes**      | C#, JavaScript                 |
| **Framework**      | .NET 8.0, ASP .NET Core        |
| **ORM**            | Entity Framework Core          |
| **Base de datos**  | SQL Server 2022 (SSMS 19)      |
| **Frontend**       | Blazor WebAssembly             |
| **Seguridad**      | JWT, cifrado de contraseñas    |
