USE BSCEvaluacionTecnica;
GO

--TABLAS.
CREATE TABLE Usuario
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
	Correo NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(256) NOT NULL,
    Rol NVARCHAR(23) CHECK (Rol IN ('Vendedor','Personal Administrativo','Administrador')) NOT NULL
);
GO
CREATE TABLE ModuloSistema(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL UNIQUE,
	Descripcion NVARCHAR(100) NOT NULL
)
GO
CREATE TABLE PermisoModuloUsuario(
	FkIdUsuario INT NOT NULL,
	FkIdModuloSistema INT NOT NULL,
	Acceso NVARCHAR(9) CHECK (Acceso IN ('Permitido','Denegado')) NOT NULL,
	PRIMARY KEY(FkIdUsuario,FkIdModuloSistema),
	CONSTRAINT FkIdUsuario FOREIGN KEY (FkIdUsuario) REFERENCES Usuario(Id),
	CONSTRAINT FkIdModuloSistema FOREIGN KEY (FkIdModuloSistema) REFERENCES ModuloSistema(Id)
)
GO
CREATE TABLE Producto
(
    Clave NVARCHAR(50) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Existencias INT NOT NULL DEFAULT 0,
	CostoUnidad DECIMAL(18,2) NOT NULL,
);
GO
CREATE TABLE Pedido
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(), --DATETIME2 para la precisión de segundos. 
    Cliente NVARCHAR(100) NOT NULL,
    FkIdVendedor INT NOT NULL,
    CONSTRAINT FKIdVendedor FOREIGN KEY (FkIdVendedor) REFERENCES Usuario(Id)
);
GO
CREATE TABLE DetallePedido
(
    FkIdPedido INT NOT NULL,
    FkClaveProducto NVARCHAR(50) NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    SubTotal DECIMAL(18,2) NOT NULL CHECK (SubTotal >= 0),
    CONSTRAINT PKDetallePedido PRIMARY KEY (FkIdPedido, FkClaveProducto),
    CONSTRAINT FKDetallePedido_Pedido FOREIGN KEY (FkIdPedido) REFERENCES Pedido(Id),
    CONSTRAINT FKDetallePedido_Producto FOREIGN KEY (FkClaveProducto) REFERENCES Producto(Clave)
);
GO

--VISTAS.
--Vista que hace joins entre Pedido, Usuario, DetallePedido y Producto que permite exponer todas las columnas necesarias para agruparlas y formatearlas desde el controlador y posteriormente enviar a la vista.
CREATE OR ALTER VIEW vw_RegistroPedidos
AS
SELECT
    p.Id AS PedidoId,
    p.FechaHora,
    p.Cliente,
    v.Id AS VendedorId,
    v.Nombre AS VendedorNombre,
    v.Correo AS VendedorCorreo,
    v.Rol AS VendedorRol,
    d.FkClaveProducto AS ProductoClave,
    prod.Nombre AS ProductoNombre,
    prod.Existencias  AS ProductoExistencias,
    prod.CostoUnidad  AS ProductoCostoUnidad,
    d.Cantidad,
    d.SubTotal
FROM Pedido p
JOIN Usuario v
    ON p.FkIdVendedor = v.Id
JOIN DetallePedido d
    ON d.FkIdPedido = p.Id
JOIN Producto prod
    ON d.FkClaveProducto = prod.Clave;
GO

--PROCEDIMIENTOS ALMACENADOS.
--Procedimiento almacenado para guardar un producto y extraer los datos para manejarlos en la vista.
CREATE OR ALTER PROCEDURE sp_GuardarProducto
   @Clave       NVARCHAR(50),
   @Nombre      NVARCHAR(100),
   @Existencias INT,
   @CostoUnidad DECIMAL(18,2)
AS
BEGIN
  SET NOCOUNT ON;
  --Insertando.
  INSERT INTO Producto(Clave, Nombre, Existencias, CostoUnidad) VALUES(@Clave, @Nombre, @Existencias, @CostoUnidad);
  --Devolviendo la fila recién insertada, para utilizarla en la vista.
  SELECT Clave, Nombre, Existencias, CostoUnidad FROM Producto WHERE Clave = @Clave;
END
GO
--Procedimiento almacenado para actualizar un producto y extraer los datos para manejarlos en la vista.
CREATE OR ALTER PROCEDURE sp_ActualizarProducto
   @Clave       NVARCHAR(50),
   @Nombre      NVARCHAR(100),
   @Existencias INT,
   @CostoUnidad DECIMAL(18,2)
AS
BEGIN
  SET NOCOUNT ON;
  --Actualizando.
  UPDATE Producto SET Nombre = @Nombre, Existencias = @Existencias, CostoUnidad = @CostoUnidad WHERE Clave = @Clave;
  --Devolviendo la fila recién actualizada, para utilizarla en la vista.
  SELECT Clave, Nombre, Existencias, CostoUnidad FROM Producto WHERE Clave = @Clave;
END
GO

--TRIGGERS.
--Trigger para automatizar la disminución de existencias en la tabla de productos, cada vez que se inserta un detalle de pedido.
CREATE OR ALTER TRIGGER tr_DisminuirExistencias
ON dbo.DetallePedido
AFTER INSERT
AS
BEGIN
  SET NOCOUNT ON;
  --Actualizando existencias.
  UPDATE p SET p.Existencias = CASE
    WHEN p.Existencias - i.Cantidad < 0 THEN 0
    ELSE p.Existencias - i.Cantidad
  END
  FROM Producto p JOIN inserted i ON p.Clave = i.FkClaveProducto;
END
GO