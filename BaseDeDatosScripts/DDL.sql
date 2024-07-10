-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- ObligatorioDB.dbo.Deposito definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.Deposito;

CREATE TABLE ObligatorioDB.dbo.Deposito (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Area nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Tamanio nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Climatizacion bit NOT NULL,
	CONSTRAINT PK_Deposito PRIMARY KEY (Id)
);


-- ObligatorioDB.dbo.Usuario definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.Usuario;

CREATE TABLE ObligatorioDB.dbo.Usuario (
	Email nvarchar(900) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NombreCompleto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Rol nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Usuario PRIMARY KEY (Email)
);


-- ObligatorioDB.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.[__EFMigrationsHistory];

CREATE TABLE ObligatorioDB.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- ObligatorioDB.dbo.RangoFechas definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.RangoFechas;

CREATE TABLE ObligatorioDB.dbo.RangoFechas (
	Id int IDENTITY(1,1) NOT NULL,
	FechaInicio datetime2 NOT NULL,
	FechaFin datetime2 NOT NULL,
	DepositoId int NULL,
	CONSTRAINT PK_RangoFechas PRIMARY KEY (Id),
	CONSTRAINT FK_RangoFechas_Deposito_DepositoId FOREIGN KEY (DepositoId) REFERENCES ObligatorioDB.dbo.Deposito(Id)
);
 CREATE NONCLUSTERED INDEX IX_RangoFechas_DepositoId ON dbo.RangoFechas (  DepositoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ObligatorioDB.dbo.Reserva definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.Reserva;

CREATE TABLE ObligatorioDB.dbo.Reserva (
	Id int IDENTITY(1,1) NOT NULL,
	ConfAdmin nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DepositoId int NULL,
	ClienteEmail nvarchar(900) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Comentario nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	RangoFechaId int NOT NULL,
	Precio float NOT NULL,
	Pago nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Reserva PRIMARY KEY (Id),
	CONSTRAINT FK_Reserva_Deposito_DepositoId FOREIGN KEY (DepositoId) REFERENCES ObligatorioDB.dbo.Deposito(Id),
	CONSTRAINT FK_Reserva_RangoFechas_RangoFechaId FOREIGN KEY (RangoFechaId) REFERENCES ObligatorioDB.dbo.RangoFechas(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Reserva_Usuario_ClienteEmail FOREIGN KEY (ClienteEmail) REFERENCES ObligatorioDB.dbo.Usuario(Email)
);
 CREATE NONCLUSTERED INDEX IX_Reserva_ClienteEmail ON dbo.Reserva (  ClienteEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Reserva_DepositoId ON dbo.Reserva (  DepositoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Reserva_RangoFechaId ON dbo.Reserva (  RangoFechaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ObligatorioDB.dbo.Promocion definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.Promocion;

CREATE TABLE ObligatorioDB.dbo.Promocion (
	Id int IDENTITY(1,1) NOT NULL,
	Etiqueta nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Descuento int NOT NULL,
	RangoFechaId int NULL,
	CONSTRAINT PK_Promocion PRIMARY KEY (Id),
	CONSTRAINT FK_Promocion_RangoFechas_RangoFechaId FOREIGN KEY (RangoFechaId) REFERENCES ObligatorioDB.dbo.RangoFechas(Id)
);
 CREATE NONCLUSTERED INDEX IX_Promocion_RangoFechaId ON dbo.Promocion (  RangoFechaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- ObligatorioDB.dbo.PromocionDeposito definition

-- Drop table

-- DROP TABLE ObligatorioDB.dbo.PromocionDeposito;

CREATE TABLE ObligatorioDB.dbo.PromocionDeposito (
	PromocionId int NOT NULL,
	DepositoId int NOT NULL,
	CONSTRAINT PK_PromocionDeposito PRIMARY KEY (PromocionId,DepositoId),
	CONSTRAINT FK_PromocionDeposito_Deposito_DepositoId FOREIGN KEY (DepositoId) REFERENCES ObligatorioDB.dbo.Deposito(Id) ON DELETE CASCADE,
	CONSTRAINT FK_PromocionDeposito_Promocion_PromocionId FOREIGN KEY (PromocionId) REFERENCES ObligatorioDB.dbo.Promocion(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_PromocionDeposito_DepositoId ON dbo.PromocionDeposito (  DepositoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;