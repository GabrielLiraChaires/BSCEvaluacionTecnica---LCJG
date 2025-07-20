USE BSCEvaluacionTecnica;
GO

--Para el registro de usuario y validación de permisos al sistema.
CREATE TABLE Usuario
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
	Correo NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(256) NOT NULL,
    Rol NVARCHAR(23) CHECK (Rol IN ('Vendedor','Personal Administrativo','Administrador')) NOT NULL
);
CREATE TABLE ModuloSistema(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL UNIQUE,
	Descripcion NVARCHAR(100) NOT NULL
)
CREATE TABLE PermisoModuloUsuario(
	FkIdUsuario INT NOT NULL,
	FkIdModuloSistema INT NOT NULL,
	Acceso NVARCHAR(9) CHECK (Acceso IN ('Permitido','Denegado')) NOT NULL,
	PRIMARY KEY(FkIdUsuario,FkIdModuloSistema),
	CONSTRAINT FkIdUsuario FOREIGN KEY (FkIdUsuario) REFERENCES Usuario(Id),
	CONSTRAINT FkIdModuloSistema FOREIGN KEY (FkIdModuloSistema) REFERENCES ModuloSistema(Id)
)

--Para la administración del negocio.
CREATE TABLE Producto
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Existencias INT NOT NULL DEFAULT 0,
	CostoUnidad DECIMAL(18,2) NOT NULL,
);
CREATE TABLE Pedido
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(), --DATETIME2 para la precisión de segundos. 
    Cliente NVARCHAR(100) NOT NULL,
    FkIdVendedor INT NOT NULL,
    CONSTRAINT FKIdVendedor FOREIGN KEY (FkIdVendedor) REFERENCES Usuario(Id)
);
CREATE TABLE DetallePedido
(
    FkIdPedido INT NOT NULL,
    FkIdProducto INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    SubTotal DECIMAL(18,2) NOT NULL CHECK (SubTotal >= 0),
    CONSTRAINT PKDetallePedido PRIMARY KEY (FkIdPedido, FkIdProducto),
    CONSTRAINT FKDetallePedido_Pedido FOREIGN KEY (FkIdPedido) REFERENCES Pedido(Id),
    CONSTRAINT FKDetallePedido_Producto FOREIGN KEY (FkIdProducto) REFERENCES Producto(Id)
);