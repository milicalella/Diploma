/* ============================================================================================
   SCRIPT DE CREACION COMPLETA DE LA BASE [is--servicios]
   --------------------------------------------------------------------------------------------
   Genera la base completa: estructura, datos, indices, defaults y claves foraneas.

   Comportamiento:
     - Si la base [is--servicios] NO existe: la crea y carga todo.
     - Si YA existe y ya tiene las tablas: no modifica nada (usa SET NOEXEC para abortar).
     - Si YA existe pero esta vacia: la inicializa (crea estructura y datos).

   Notas de esta version respecto del script original de SSMS:
     1. CREATE DATABASE protegido con IF DB_ID(...) IS NULL.
     2. Se dejo UN SOLO indice unico sobre Usuario.Email (la base original tenia 7 duplicados).
     3. Las tablas de relaciones de seguridad se cargan sin filas duplicadas
        (Familia_Familia, Familia_Patente, Rol_Familia, Rol_Patente).
     4. Las claves foraneas se crean al final, despues de la carga de datos.

   Servidor destino: .\SQLEXPRESS  |  SQL Server 2022
   ============================================================================================ */

USE [master]
GO

SET NOEXEC OFF   -- Por si quedo activo de una ejecucion anterior interrumpida.
GO

IF DB_ID(N'is--servicios') IS NULL
BEGIN
    PRINT N'Creando la base [is--servicios]...';
    CREATE DATABASE [is--servicios];
END
ELSE
BEGIN
    PRINT N'La base [is--servicios] ya existe. Se verificara si requiere inicializacion.';
END
GO

USE [is--servicios]
GO

/* Guarda de idempotencia: si las tablas ya estan creadas, se aborta el resto del script.
   Debe ejecutarse despues del USE, porque SET NOEXEC tambien saltea los USE y el
   contexto quedaria en master (provocando errores de compilacion en los INSERT). */
IF OBJECT_ID(N'dbo.Usuario', N'U') IS NOT NULL
BEGIN
    PRINT N'La base [is--servicios] ya esta inicializada. El script no realizara ningun cambio.';
    SET NOEXEC ON;
END
ELSE
BEGIN
    PRINT N'Inicializando estructura y datos de [is--servicios]...';
END
GO

/* ============================================================================================
   1. TABLAS
   ============================================================================================ */

/****** Object: Table [dbo].[BitacoraEventos] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BitacoraEventos](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [DNI] [varchar](50) NOT NULL,
    [Evento] [varchar](255) NOT NULL,
    [Criticidad] [int] NOT NULL,
    [Modulo] [varchar](50) NOT NULL,
    [FechaHora] [datetime] NOT NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Cliente] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
    [DNI] [varchar](50) NOT NULL,
    [Nombre] [varchar](50) NOT NULL,
    [Apellido] [varchar](50) NOT NULL,
    [Telefono] [varchar](50) NULL,
    [Email] [varchar](100) NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [DNI] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Contrato] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contrato](
    [NroContrato] [varchar](20) NOT NULL,
    [FechaEmision] [date] NOT NULL,
    [DNIPropietario] [varchar](50) NOT NULL,
    [DNICliente] [varchar](50) NOT NULL,
    [CodigoPropiedad] [int] NOT NULL,
    [TipoContrato] [varchar](50) NOT NULL,
    [FechaInicio] [date] NOT NULL,
    [FechaFin] [date] NOT NULL,
    [MontoAcordado] [decimal](18, 2) NOT NULL,
    [FormaPago] [varchar](50) NOT NULL,
    [DepositoGarantia] [decimal](18, 2) NOT NULL,
    [Estado] [varchar](20) NOT NULL,
    [DVH] [bigint] NULL,
    CONSTRAINT [PK_Contrato] PRIMARY KEY CLUSTERED
    (
        [NroContrato] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[DigitoVerificador] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DigitoVerificador](
    [NombreTabla] [varchar](50) NOT NULL,
    [DVV] [bigint] NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [NombreTabla] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Familia] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [varchar](50) NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Familia_Familia] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia_Familia](
    [IdFamiliaPadre] [int] NOT NULL,
    [IdFamiliaHija] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Familia_Patente] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familia_Patente](
    [IdFamilia] [int] NOT NULL,
    [IdPatente] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Idioma] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idioma](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [varchar](5) NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Patente] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patente](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [varchar](50) NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Propiedad] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Propiedad](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Direccion] [varchar](150) NOT NULL,
    [Tipo] [varchar](50) NOT NULL,
    [Estado] [varchar](50) NOT NULL,
    [Precio] [decimal](18, 2) NOT NULL,
    [SuperficieM2] [decimal](10, 2) NOT NULL,
    [Ambientes] [int] NOT NULL,
    [Dormitorios] [int] NOT NULL,
    [Banios] [int] NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Rol] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [varchar](50) NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Rol_Familia] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Familia](
    [IdRol] [int] NOT NULL,
    [IdFamilia] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Rol_Patente] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Patente](
    [IdRol] [int] NOT NULL,
    [IdPatente] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Usuario] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
    [DNI] [varchar](50) NOT NULL,
    [Nombre] [varchar](50) NOT NULL,
    [Apellido] [varchar](50) NOT NULL,
    [Username] [varchar](50) NOT NULL,
    [Email] [varchar](100) NOT NULL,
    [PasswordHash] [varchar](255) NOT NULL,
    [Activo] [bit] NULL,
    [Bloqueo] [bit] NULL,
    [Intentos] [int] NULL,
    [IdRol] [int] NOT NULL,
    [UltimoIntentoFallido] [datetime] NULL,
    [IdIdioma] [int] NOT NULL,
    [DVH] [bigint] NULL,
    PRIMARY KEY CLUSTERED
    (
        [DNI] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object: Table [dbo].[Visita] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visita](
    [IdVisita] [int] IDENTITY(1,1) NOT NULL,
    [IdPropiedad] [int] NOT NULL,
    [DNICliente] [varchar](8) NOT NULL,
    [Fecha] [date] NOT NULL,
    [HoraInicio] [time](0) NOT NULL,
    [HoraFin] [time](0) NOT NULL,
    [Estado] [varchar](15) NOT NULL,
    [DVH] [bigint] NULL,
    [Observaciones] [varchar](500) NULL,
    PRIMARY KEY CLUSTERED
    (
        [IdVisita] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/* ============================================================================================
   2. DATOS
   ============================================================================================ */

/* BitacoraEventos (539 fila(s)) */
SET IDENTITY_INSERT [dbo].[BitacoraEventos] ON 
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (6, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:19:37.597' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (7, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:21:05.077' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (8, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:27:25.610' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (9, N'45813074', N'Se creo un usuario nuevo', 2, N'1', CAST(N'2026-05-17T11:28:25.577' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (10, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:32:23.743' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (11, N'45813074', N'Se modificó usuario DNI 47006530', 2, N'1', CAST(N'2026-05-17T11:32:43.997' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (12, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:40:30.300' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (13, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:43:57.870' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (14, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:53:05.233' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (15, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:54:09.070' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (16, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:58:13.990' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (17, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T11:59:08.243' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (18, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T12:00:16.940' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (19, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T12:01:12.617' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (20, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T12:07:35.503' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (21, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:42:51.183' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (22, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:45:45.357' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (23, N'47006530', N'Se desactivó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T21:45:56.000' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (24, N'47006530', N'Se activó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T21:46:32.520' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (25, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:49:02.807' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (26, N'47006530', N'Se desactivó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T21:49:09.807' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (27, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:50:25.993' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (28, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:53:33.337' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (29, N'47006530', N'Se activó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T21:53:47.800' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (30, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T21:54:11.067' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (31, N'45813074', N'Usuario: naiara45813074 bloqueado.', 3, N'2', CAST(N'2026-05-17T22:06:59.253' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (32, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:07:32.830' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (33, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:10:45.197' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (34, N'47006530', N'Se desbloqueó el usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T22:10:58.273' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (35, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:25:08.600' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (36, N'47006530', N'Se desactivó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-17T22:25:55.290' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (37, N'47006530', N'Se creo un usuario nuevo', 2, N'1', CAST(N'2026-05-17T22:26:52.600' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (38, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:27:44.820' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (39, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:28:33.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (40, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:34:11.293' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (41, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:35:02.120' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (42, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:35:13.907' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (43, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-17T22:48:43.760' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (44, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T11:42:55.747' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (45, N'47006530', N'Se activó la cuenta del usuario: naiara45813074', 3, N'1', CAST(N'2026-05-18T11:44:19.873' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (46, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T14:27:45.077' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (47, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T14:28:56.557' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (48, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T15:01:21.063' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (49, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T15:03:12.130' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (50, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T15:05:25.477' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (51, N'21487343', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-18T15:05:59.593' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (52, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T12:27:13.143' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (53, N'47006530', N'Se desactivó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T12:28:07.067' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (54, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:32:29.370' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (55, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:39:05.160' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (56, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:39:10.193' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (57, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:39:51.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (58, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:42:01.617' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (59, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:45:55.563' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (60, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:52:05.663' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (61, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:53:34.013' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (62, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T14:58:11.810' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (63, N'47006530', N'Se modificó usuario DNI 45813074', 2, N'1', CAST(N'2026-05-19T14:58:29.923' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (64, N'47006530', N'Se modificó usuario DNI 45813074', 2, N'1', CAST(N'2026-05-19T14:58:46.800' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (65, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:05:15.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (66, N'47006530', N'Se activó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T15:05:29.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (67, N'47006530', N'Se desactivó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T15:05:34.830' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (68, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:16:41.457' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (69, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:33:11.453' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (70, N'47006530', N'Se activó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T15:33:51.380' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (71, N'47006530', N'Se desactivó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T15:33:56.527' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (72, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:49:23.810' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (73, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:54:44.080' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (74, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T15:58:04.930' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (75, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:05:53.597' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (76, N'45813074', N'Usuario naiara45813074 bloqueado.', 3, N'2', CAST(N'2026-05-19T16:08:39.250' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (77, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:08:51.870' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (78, N'47006530', N'Se desbloqueó el usuario: naiara45813074', 3, N'1', CAST(N'2026-05-19T16:09:18.847' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (79, N'47006530', N'Se activó la cuenta del usuario: marisa21487343', 3, N'1', CAST(N'2026-05-19T16:09:25.423' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (80, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:09:48.050' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (81, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:18:23.687' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (82, N'47006530', N'Se modificó usuario DNI 45813074', 2, N'1', CAST(N'2026-05-19T16:18:41.130' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (83, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:44:33.090' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (84, N'21487343', N'Usuario marisa21487343 bloqueado.', 3, N'2', CAST(N'2026-05-19T16:45:05.373' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (85, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:45:28.503' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (86, N'45813074', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T16:49:10.297' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (87, N'47006530', N'Realizo login exitoso.', 3, N'1', CAST(N'2026-05-19T21:36:14.970' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (88, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T00:56:03.857' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (89, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T01:40:37.407' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (90, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T01:51:39.043' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (91, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:01:40.010' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (92, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:04:29.677' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (93, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:07:01.263' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (94, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:09:37.437' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (95, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:13:27.983' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (96, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:19:09.010' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (97, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:23:09.917' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (98, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T02:24:59.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (99, N'47006530', N'Usuario benjamin47006530 bloqueado.', 3, N'2', CAST(N'2026-05-20T03:37:07.130' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (100, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T03:38:03.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (101, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T03:38:45.070' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (102, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T03:39:07.207' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (103, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T08:02:33.730' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (104, N'45813074', N'Se desbloqueó el usuario: benjamin47006530', 3, N'1', CAST(N'2026-05-20T08:02:46.063' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (105, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T08:03:49.957' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (106, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-20T08:04:30.153' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (107, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-24T13:55:48.057' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (108, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-27T10:00:44.997' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (109, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-05-27T10:03:56.040' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (110, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:31:33.903' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (111, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:31:57.737' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (112, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:34:29.113' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (113, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:36:02.683' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (114, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:38:29.403' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (115, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:39:44.740' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (116, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:40:06.457' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (117, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:40:41.917' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (118, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:43:25.243' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (119, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:43:53.933' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (120, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:44:34.383' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (121, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:44:47.320' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (122, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:45:01.473' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (123, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:45:14.603' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (124, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:53:06.797' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (125, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:53:23.393' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (126, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:53:49.720' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (127, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:56:31.570' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (128, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:56:57.730' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (129, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:57:13.133' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (130, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T21:57:30.380' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (131, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:01:56.377' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (132, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:02:30.913' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (133, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:04:04.773' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (134, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:05:20.070' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (135, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:07:42.730' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (136, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:07:57.630' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (137, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:09:01.920' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (138, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:09:26.147' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (139, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:12:06.740' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (140, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:12:25.447' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (141, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:13:48.497' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (142, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:14:43.830' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (143, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:16:05.860' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (144, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:16:28.557' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (145, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:16:49.230' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (146, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:20:37.400' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (147, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:20:52.803' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (148, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:21:05.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (149, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:21:19.433' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (150, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:22:55.460' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (151, N'45813074', N'Usuario naiara45813074 bloqueado.', 3, N'2', CAST(N'2026-06-02T22:27:30.460' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (152, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:28:28.633' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (153, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:36:59.443' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (154, N'47006530', N'Se desbloqueó el usuario: naiara45813074', 3, N'1', CAST(N'2026-06-02T22:37:07.600' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (155, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:37:55.457' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (156, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:38:34.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (157, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:38:46.263' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (158, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-02T22:39:11.277' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (159, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-03T08:16:34.670' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (160, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-03T08:17:14.217' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (161, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-03T12:02:34.690' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (162, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-03T12:04:11.183' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (163, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-10T10:41:14.297' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (164, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-10T10:41:33.323' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (165, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T12:58:15.097' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (166, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T12:58:49.657' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (167, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T13:01:14.290' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (168, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T13:01:36.627' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (169, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T16:11:34.677' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (170, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T22:59:08.167' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (171, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-17T08:40:26.480' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (172, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-19T16:15:51.707' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (173, N'47006530', N'Elimino una familia.', 3, N'1', CAST(N'2026-06-09T13:05:54.693' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (174, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-09T13:06:25.420' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (175, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-09T13:07:10.963' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (176, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-09T13:15:24.757' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (177, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-09T17:13:57.740' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (178, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-10T10:15:47.420' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (179, N'47006530', N'Creo una nueva familia', 3, N'1', CAST(N'2026-06-10T10:16:20.027' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (180, N'47006530', N'Asigno la familia Usuario a la familia prueba.', 3, N'1', CAST(N'2026-06-10T10:16:20.063' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (181, N'47006530', N'Creo una nueva familia', 3, N'1', CAST(N'2026-06-10T10:16:48.133' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (182, N'47006530', N'Asigno la familia prueba a el rol Prueba.', 3, N'1', CAST(N'2026-06-10T10:16:48.167' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (183, N'47006530', N'Asigno la patente Gestion Roles a el rol Prueba.', 3, N'1', CAST(N'2026-06-10T10:17:00.957' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (184, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T20:11:39.360' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (185, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T20:21:30.730' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (186, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-15T20:21:47.070' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (187, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-21T22:56:18.720' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (188, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-21T22:56:38.220' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (189, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-21T22:58:33.760' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (190, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-21T22:58:45.493' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (191, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-22T19:30:32.217' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (192, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-22T19:31:33.737' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (193, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-22T19:47:48.487' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (194, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-22T19:53:08.517' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (195, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-23T23:40:29.947' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (196, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T07:50:15.420' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (197, N'47006530', N'Asignó la patente Cerrar Sesion a la familia Familia prueba.', 3, N'3', CAST(N'2026-06-24T07:51:46.007' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (198, N'47006530', N'Creo una nueva familia', 3, N'3', CAST(N'2026-06-24T07:51:46.013' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (199, N'47006530', N'Asigno la patente Gestion Familia a la familia Familia prueba.', 3, N'3', CAST(N'2026-06-24T07:52:34.423' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (200, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T07:57:24.290' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (201, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T07:58:48.510' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (202, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T08:00:11.187' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (203, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T08:01:00.643' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (204, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-06-24T08:02:01.803' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (205, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-06T22:24:56.653' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (206, N'47006530', N'Se creo un usuario nuevo', 2, N'1', CAST(N'2026-07-06T22:26:16.317' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (207, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T00:59:28.797' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (208, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:00:03.440' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (209, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:04:48.423' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (210, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:08:27.627' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (211, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:11:06.700' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (212, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T01:11:06.723' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (213, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T01:11:12.497' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (214, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:12:05.080' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (215, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:17:18.263' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (216, N'47307577', N'Se detectó una inconsistencia en la tabla Usuario.', 3, N'2', CAST(N'2026-07-07T01:17:18.287' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (217, N'47307577', N'Se detectó una inconsistencia en la tabla Familia.', 3, N'2', CAST(N'2026-07-07T01:17:18.297' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (218, N'47307577', N'Se detectó una inconsistencia en la tabla Patente.', 3, N'2', CAST(N'2026-07-07T01:17:18.303' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (219, N'47307577', N'Se reparó la tabla Usuario.', 3, N'2', CAST(N'2026-07-07T01:18:08.003' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (220, N'47307577', N'Se reparó la tabla Familia.', 3, N'2', CAST(N'2026-07-07T01:18:08.020' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (221, N'47307577', N'Se reparó la tabla Patente.', 3, N'2', CAST(N'2026-07-07T01:18:08.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (222, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:29:50.933' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (223, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T01:29:50.960' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (224, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T01:29:54.393' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (225, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T01:30:40.457' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (226, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T17:54:37.353' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (227, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T17:54:37.720' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (228, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T17:55:54.567' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (229, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T17:58:24.327' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (230, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:04:29.127' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (231, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:04:29.233' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (232, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:04:30.987' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (233, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:04:41.953' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (234, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:07:22.787' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (235, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:11:07.703' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (236, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:12:41.240' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (237, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:24:34.943' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (238, N'47307577', N'Se detectó una inconsistencia en la tabla Usuario.', 3, N'2', CAST(N'2026-07-07T18:24:35.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (239, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:24:35.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (240, N'47307577', N'Se detectó una inconsistencia en la tabla Familia.', 3, N'2', CAST(N'2026-07-07T18:24:35.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (241, N'47307577', N'Se detectó una inconsistencia en la tabla Patente.', 3, N'2', CAST(N'2026-07-07T18:24:35.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (242, N'47307577', N'Se reparó la tabla Usuario.', 3, N'2', CAST(N'2026-07-07T18:24:40.773' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (243, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:24:40.793' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (244, N'47307577', N'Se reparó la tabla Familia.', 3, N'2', CAST(N'2026-07-07T18:24:40.813' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (245, N'47307577', N'Se reparó la tabla Patente.', 3, N'2', CAST(N'2026-07-07T18:24:40.857' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (246, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:24:57.110' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (247, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:26:11.340' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (248, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:26:11.437' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (249, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:26:13.083' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (250, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:29:27.797' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (251, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:29:27.877' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (252, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T18:29:29.807' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (253, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:29:44.067' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (254, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T18:29:53.920' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (255, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:19:30.793' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (256, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:20:10.507' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (257, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:26:32.547' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (258, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:28:28.420' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (259, N'47307577', N'Asignó la patente Cerrar Sesion a la familia Prueba2.', 3, N'3', CAST(N'2026-07-07T19:30:03.960' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (260, N'47307577', N'Creo una nueva familia', 3, N'3', CAST(N'2026-07-07T19:30:03.973' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (261, N'47307577', N'Elimino una familia.', 3, N'3', CAST(N'2026-07-07T19:30:20.837' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (262, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:33:22.333' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (263, N'47307577', N'Se detectó una inconsistencia en la tabla Familia.', 3, N'2', CAST(N'2026-07-07T19:33:22.467' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (264, N'47307577', N'Se reparó la tabla Familia.', 3, N'2', CAST(N'2026-07-07T19:33:38.377' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (265, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:33:48.250' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (266, N'47307577', N'Elimino una familia.', 3, N'3', CAST(N'2026-07-07T19:34:17.737' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (267, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:35:03.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (268, N'47307577', N'Se detectó una inconsistencia en la tabla Familia.', 3, N'2', CAST(N'2026-07-07T19:35:03.120' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (269, N'47307577', N'Se reparó la tabla Familia.', 3, N'2', CAST(N'2026-07-07T19:35:14.040' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (270, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:35:25.220' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (271, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:35:57.357' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (272, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:52:34.197' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (273, N'47307577', N'Elimino una familia.', 3, N'3', CAST(N'2026-07-07T19:52:49.600' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (274, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:53:09.957' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (275, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:53:57.157' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (276, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:54:29.043' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (277, N'47307577', N'Se modificó usuario DNI 45813074', 2, N'1', CAST(N'2026-07-07T19:54:45.817' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (278, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:55:00.857' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (279, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T19:57:18.070' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (280, N'45813074', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T19:57:18.193' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (281, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T20:03:28.503' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (282, N'45813074', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T20:03:28.980' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (283, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T20:05:04.003' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (284, N'45813074', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T20:05:04.123' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (285, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T20:05:15.910' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (286, N'47307577', N'Se detectó una inconsistencia en la tabla Rol.', 3, N'2', CAST(N'2026-07-07T20:05:15.937' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (287, N'47307577', N'Se reparó la tabla Rol.', 3, N'2', CAST(N'2026-07-07T20:05:18.013' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (288, N'45813074', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-07T20:05:28.153' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (289, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T00:15:06.097' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (290, N'47006530', N'Se detectó una inconsistencia en la tabla Patente.', 3, N'2', CAST(N'2026-07-08T00:15:06.213' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (291, N'47006530', N'Se reparó la tabla Patente.', 3, N'2', CAST(N'2026-07-08T00:15:07.543' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (292, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T00:15:16.193' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (293, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T00:15:59.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (294, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T08:22:08.050' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (295, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T08:36:45.597' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (296, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T09:00:55.190' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (297, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T09:01:36.053' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (298, N'47307577', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-07-08T20:36:13.787' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (299, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T14:37:29.803' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (300, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:05:26.797' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (301, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:13:34.087' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (302, N'47006530', N'Se creo un usuario nuevo', 2, N'1', CAST(N'2026-09-26T15:17:02.117' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (303, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:18:23.557' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (304, N'45000111', N'Se detectó una inconsistencia en la tabla Usuario.', 3, N'2', CAST(N'2026-09-26T15:18:23.573' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (305, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:18:58.157' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (306, N'45000111', N'Se detectó una inconsistencia en la tabla Usuario.', 3, N'2', CAST(N'2026-09-26T15:18:58.260' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (307, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:19:34.923' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (308, N'47006530', N'Se detectó una inconsistencia en la tabla Usuario.', 3, N'2', CAST(N'2026-09-26T15:19:34.977' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (309, N'47006530', N'Se reparó la tabla Usuario.', 3, N'2', CAST(N'2026-09-26T15:19:38.340' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (310, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:19:47.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (311, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:20:27.613' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (312, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-26T15:21:42.573' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (313, N'45000111', N'Se registró el cliente Dolores Noguera (DNI 45111000).', 2, N'4', CAST(N'2026-09-26T15:22:39.400' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (314, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:14:22.080' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (315, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:15:35.427' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (316, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:21:22.470' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (317, N'45000111', N'Se detectó una inconsistencia en la tabla Patente.', 3, N'2', CAST(N'2026-09-27T18:21:22.827' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (318, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:22:21.033' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (319, N'47006530', N'Se detectó una inconsistencia en la tabla Patente.', 3, N'2', CAST(N'2026-09-27T18:22:21.057' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (320, N'47006530', N'Se reparó la tabla Patente.', 3, N'2', CAST(N'2026-09-27T18:22:23.907' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (321, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:22:37.873' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (322, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:29:24.927' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (323, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:33:10.883' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (324, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:33:42.737' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (325, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:34:11.510' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (326, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:34:53.803' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (327, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:34:59.543' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (328, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:35:03.110' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (329, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:35:06.127' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (330, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:35:09.483' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (331, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T18:35:12.923' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (332, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:48:55.387' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (333, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T18:58:05.417' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (334, N'45000111', N'Consultó la disponibilidad de la propiedad 10 para el 29/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T18:59:09.850' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (335, N'45000111', N'Consultó la disponibilidad de la propiedad 10 para el 29/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T19:02:34.877' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (336, N'45000111', N'Consultó la disponibilidad de la propiedad 10 para el 29/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T19:02:50.413' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (337, N'45000111', N'Consultó la disponibilidad de la propiedad 10 para el 29/09/2026 en el horario 09:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T19:02:55.887' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (338, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 29/09/2026 en el horario 09:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T19:03:03.930' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (339, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T22:55:12.407' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (340, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T22:55:33.820' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (341, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T22:59:42.377' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (342, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T22:59:57.390' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (343, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:02:27.760' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (344, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T23:02:38.900' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (345, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 28/09/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-27T23:02:40.903' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (346, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:13:08.050' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (347, N'45000111', N'Se actualizó el estado de la visita 1 a ''Realizada'' para el cliente 31111222.', 2, N'6', CAST(N'2026-09-27T23:13:40.420' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (348, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:16:40.293' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (349, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:20:21.863' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (350, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:23:12.867' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (351, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:29:19.860' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (352, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:29:41.087' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (353, N'45000111', N'Consultó la disponibilidad de la propiedad 11 para el 30/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-27T23:30:15.790' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (354, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 11 el 30/09/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-27T23:30:18.027' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (355, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:49:19.820' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (356, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-27T23:52:46.313' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (357, N'45000111', N'Se registró el cliente Alma Castro Barrio (DNI 46948480).', 2, N'4', CAST(N'2026-09-27T23:54:34.823' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (358, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:55:09.737' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (359, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:55:24.213' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (360, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:55:25.397' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (361, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:55:34.473' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (362, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:56:38.107' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (363, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:56:46.467' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (364, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:56:49.747' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (365, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 0 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:56:50.900' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (366, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-27T23:56:54.090' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (367, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T00:00:02.063' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (368, N'45000111', N'Se registró el cliente Tomas Calella (DNI 12345678).', 2, N'4', CAST(N'2026-09-28T00:00:44.493' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (369, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-28T00:00:52.013' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (370, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 10:00-11:00: ocupada.', 2, N'6', CAST(N'2026-09-28T00:01:55.087' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (371, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 10:00-13:00: ocupada.', 2, N'6', CAST(N'2026-09-28T00:02:02.947' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (372, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 28/09/2026 en el horario 11:00-13:00: disponible.', 2, N'6', CAST(N'2026-09-28T00:02:06.130' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (373, N'45000111', N'Se registró una visita Pendiente del cliente 12345678 para la propiedad 9 el 28/09/2026 (11:00 - 13:00).', 2, N'6', CAST(N'2026-09-28T00:02:17.200' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (374, N'45000111', N'Se actualizó el estado de la visita 6 a ''Cancelada'' para el cliente 12345678.', 2, N'6', CAST(N'2026-09-28T00:08:38.637' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (375, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T10:12:53.430' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (376, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 08/10/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T10:13:33.847' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (377, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 08/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T10:13:36.467' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (378, N'45000111', N'Se actualizó el estado de la visita 7 a ''Cancelada'' para el cliente 45111000.', 2, N'6', CAST(N'2026-09-28T10:13:49.320' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (379, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T16:44:50.197' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (380, N'45000111', N'Se registró el cliente Milagros Araujo (DNI 46307577).', 2, N'4', CAST(N'2026-09-28T16:46:11.440' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (381, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 1 resultado(s).', 2, N'5', CAST(N'2026-09-28T16:46:31.067' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (382, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 30/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T16:46:41.340' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (383, N'45000111', N'Se registró una visita Pendiente del cliente 46307577 para la propiedad 9 el 30/09/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T16:46:41.367' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (384, N'45000111', N'Se actualizó el estado de la visita 4 a ''Realizada'' para el cliente 45111000.', 2, N'6', CAST(N'2026-09-28T16:47:14.590' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (385, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T16:54:18.313' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (386, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:00:04.580' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (387, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:01:30.713' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (388, N'45000111', N'Se registró el cliente Benjamin Calella (DNI 98765432).', 2, N'4', CAST(N'2026-09-28T17:02:19.700' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (389, N'45000111', N'Consultó la disponibilidad de la propiedad 11 para el 09/10/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T17:02:35.187' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (390, N'45000111', N'Se registró una visita Pendiente del cliente 98765432 para la propiedad 11 el 09/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T17:02:35.217' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (391, N'45000111', N'Se actualizó el estado de la visita 9 a ''Cancelada'' para el cliente 98765432.', 2, N'6', CAST(N'2026-09-28T17:04:33.360' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (392, N'45000111', N'Se actualizó el estado de la visita 8 a ''Realizada'' para el cliente 46307577.', 2, N'6', CAST(N'2026-09-28T17:05:55.563' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (393, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:12:54.163' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (394, N'45000111', N'Consultó la disponibilidad de la propiedad 11 para el 28/09/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T17:13:24.520' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (395, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 11 el 28/09/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T17:13:24.533' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (396, N'45000111', N'Se registró el cliente Rufina Calella (DNI 88765123).', 2, N'4', CAST(N'2026-09-28T17:14:10.637' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (397, N'45000111', N'Consultó la disponibilidad de la propiedad 10 para el 09/10/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T17:14:22.400' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (398, N'45000111', N'Se registró una visita Pendiente del cliente 88765123 para la propiedad 10 el 09/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T17:14:22.417' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (399, N'45000111', N'Se actualizó el estado de la visita 11 a ''Realizada'' para el cliente 88765123.', 2, N'6', CAST(N'2026-09-28T17:14:38.970' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (400, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:26:09.270' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (401, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:33:04.007' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (402, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:36:22.040' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (403, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:37:13.520' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (404, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:40:59.267' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (405, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T17:47:54.690' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (406, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T22:32:01.170' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (407, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:40:45.507' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (408, N'45000111', N'Se registró el cliente Juan Calella (DNI 34678912).', 2, N'4', CAST(N'2026-09-28T23:41:54.877' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (409, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 07/10/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T23:42:36.260' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (410, N'45000111', N'Se registró una visita Pendiente del cliente 34678912 para la propiedad 9 el 07/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T23:42:36.290' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (411, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:45:37.157' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (412, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:48:00.980' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (413, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:53:15.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (414, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:54:07.883' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (415, N'45000111', N'Se registró el cliente Pilar Nashe (DNI 47390789).', 2, N'4', CAST(N'2026-09-28T23:55:49.113' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (416, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-28T23:56:00.597' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (417, N'45000111', N'Consultó la disponibilidad de la propiedad 9 para el 08/10/2026 en el horario 10:00-11:00: disponible.', 2, N'6', CAST(N'2026-09-28T23:56:17.110' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (418, N'45000111', N'Se registró una visita Pendiente del cliente 47390789 para la propiedad 9 el 08/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-28T23:56:17.133' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (419, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-28T23:57:36.390' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (420, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:12:01.603' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (421, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:23:58.233' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (422, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:38:42.327' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (423, N'45000111', N'Se modificó el cliente Pilar Moreno (DNI 47390789).', 2, N'4', CAST(N'2026-09-29T00:39:22.817' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (424, N'45000111', N'Se modificó el cliente Pilar Moreno (DNI 47390789).', 2, N'4', CAST(N'2026-09-29T00:39:32.727' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (425, N'45000111', N'Se modificó el cliente Pilar Moreno (DNI 47390789).', 2, N'4', CAST(N'2026-09-29T00:40:00.633' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (426, N'45000111', N'Se modificó el cliente Pilar Moreno (DNI 47390789).', 2, N'4', CAST(N'2026-09-29T00:40:27.990' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (427, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:45:34.507' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (428, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:46:31.050' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (429, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:48:10.470' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (430, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:51:58.600' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (431, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T00:53:15.670' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (432, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:04:29.730' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (433, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:05:35.063' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (434, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:08:16.060' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (435, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:10:32.057' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (436, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:13:06.837' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (437, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:15:20.140' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (438, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:17:50.190' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (439, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T01:20:30.090' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (440, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T13:31:32.503' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (441, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-29T13:31:53.553' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (442, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-29T13:31:55.770' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (443, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-29T13:31:56.820' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (444, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-29T13:31:57.687' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (445, N'45000111', N'Consultó el catálogo de propiedades y obtuvo 3 resultado(s).', 2, N'5', CAST(N'2026-09-29T13:32:01.877' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (446, N'45000111', N'Se modificó el cliente Juan Calella (DNI 34678912).', 2, N'4', CAST(N'2026-09-29T13:32:18.103' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (447, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T19:11:18.347' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (448, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 11/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T19:11:42.753' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (449, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 31/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T19:13:35.333' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (450, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T19:20:30.143' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (451, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 05/11/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T19:20:56.230' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (452, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T19:49:48.060' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (453, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 22/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T19:50:03.633' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (454, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T19:56:13.150' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (455, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 11 el 26/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T19:56:30.667' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (456, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T19:59:49.147' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (457, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 17/12/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T20:00:06.317' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (458, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T20:03:06.763' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (459, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 20/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T20:03:22.647' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (460, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T20:06:03.670' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (461, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 04/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T20:06:17.943' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (462, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T20:14:34.477' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (463, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 10/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T20:14:47.923' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (464, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T21:43:50.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (465, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T21:44:43.197' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (466, N'45000111', N'Se registró el cliente Esteban Ross (DNI 29779483).', 2, N'4', CAST(N'2026-09-29T21:47:38.810' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (467, N'45000111', N'Se modificó el cliente Esteban Ros (DNI 29779483).', 2, N'4', CAST(N'2026-09-29T21:47:46.827' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (468, N'45000111', N'Se registró una visita Pendiente del cliente 29779483 para la propiedad 10 el 18/11/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-29T21:50:20.193' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (469, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T22:41:25.213' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (470, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T22:42:24.067' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (471, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T22:46:11.207' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (473, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T22:54:38.723' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (474, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T23:01:34.400' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (475, N'45000111', N'Se registró el cliente Melina Esquivel (DNI 23678965).', 2, N'4', CAST(N'2026-09-29T23:02:16.823' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (476, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T23:22:01.093' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (477, N'45000111', N'Se registró la propiedad 14 - Int. Amenedo 148.', 2, N'5', CAST(N'2026-09-29T23:22:39.130' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (478, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T23:42:50.670' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (479, N'45000111', N'Se actualizó el estado de la visita 15 a ''Realizada'' para el cliente 45111000.', 2, N'6', CAST(N'2026-09-29T23:43:28.253' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (480, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-29T23:58:07.907' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (481, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T00:02:37.423' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (482, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 14 el 10/10/2026 (11:00 - 12:00).', 2, N'6', CAST(N'2026-09-30T00:03:06.967' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (483, N'45000111', N'Se registró el cliente Mili Calella (DNI 47307577).', 2, N'4', CAST(N'2026-09-30T00:04:22.967' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (484, N'45000111', N'Se registró una visita Pendiente del cliente 47307577 para la propiedad 10 el 14/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-30T00:04:40.340' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (485, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T00:09:28.883' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (486, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T01:48:34.817' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (487, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 14 el 30/09/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-30T01:49:37.003' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (488, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T01:51:24.507' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (489, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 9 el 30/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-30T01:52:11.477' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (490, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T01:53:06.597' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (491, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T02:06:00.177' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (492, N'45000111', N'Se registró el cliente Daniel Babio (DNI 23456789).', 2, N'4', CAST(N'2026-09-30T02:06:49.147' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (493, N'45000111', N'Se registró una visita Pendiente del cliente 23456789 para la propiedad 11 el 30/09/2026 (12:00 - 13:00).', 2, N'6', CAST(N'2026-09-30T02:07:22.180' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (494, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-09-30T17:42:42.910' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (495, N'45000111', N'Se registró una visita Pendiente del cliente 45111000 para la propiedad 10 el 30/01/2027 (10:00 - 11:00).', 2, N'6', CAST(N'2026-09-30T17:43:57.450' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (496, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-27T22:21:16.333' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (500, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T11:09:33.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (501, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T19:58:55.887' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (502, N'45000111', N'Se registró el cliente Benjamin Aguila (DNI 45178900).', 2, N'4', CAST(N'2026-10-28T19:59:41.040' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (507, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T20:27:10.413' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (508, N'45000111', N'Se generó el contrato C-20261028-0001 (propietario DNI 45111000, cliente DNI 47307577, propiedad 14).', 2, N'7', CAST(N'2026-10-28T20:28:22.773' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (509, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T20:28:56.047' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (634, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T21:35:49.690' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (635, N'45000111', N'Se generó el contrato C-20261028-0002 (propietario DNI 45111000, cliente DNI 47307577, propiedad 11).', 2, N'7', CAST(N'2026-10-28T21:36:36.860' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (636, N'45000111', N'Se actualizó el contrato C-20261028-0002 a ''Firmado'' (propietario DNI 45111000, cliente DNI 47307577).', 2, N'7', CAST(N'2026-10-28T21:37:31.193' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (637, N'45000111', N'Se actualizó la propiedad 11 a ''Vendida'' por la firma del contrato C-20261028-0002.', 2, N'5', CAST(N'2026-10-28T21:37:31.207' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (638, N'45000111', N'Se actualizó el contrato C-20261028-0001 a ''Firmado'' (propietario DNI 45111000, cliente DNI 47307577).', 2, N'7', CAST(N'2026-10-28T21:39:00.557' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (639, N'45000111', N'Se actualizó la propiedad 14 a ''Alquilada'' por la firma del contrato C-20261028-0001.', 2, N'5', CAST(N'2026-10-28T21:39:00.557' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (640, N'45000111', N'Se registró una visita Pendiente del cliente 47307577 para la propiedad 9 el 28/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-10-28T21:40:26.830' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (641, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-10-28T21:59:11.927' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (642, N'45000111', N'Se generó el contrato C-20261028-0003 (propietario DNI 45111000, cliente DNI 47307577, propiedad 11).', 2, N'7', CAST(N'2026-10-28T22:00:12.347' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (643, N'45000111', N'Se actualizó el contrato C-20261028-0003 a ''Cancelado''. La propiedad 11 mantiene su disponibilidad.', 2, N'7', CAST(N'2026-10-28T22:00:58.550' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (644, N'45000111', N'Se generó el contrato C-20261028-0004 (propietario DNI 45111000, cliente DNI 47307577, propiedad 11).', 2, N'7', CAST(N'2026-10-28T22:02:01.807' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (645, N'45000111', N'Se generó el contrato C-20261028-0005 (propietario DNI 47307577, cliente DNI 45111000, propiedad 9).', 2, N'7', CAST(N'2026-10-28T22:02:49.877' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (646, N'45000111', N'Se actualizó el contrato C-20261028-0005 a ''Firmado'' (propietario DNI 47307577, cliente DNI 45111000).', 2, N'7', CAST(N'2026-10-28T22:03:04.203' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (647, N'45000111', N'Se actualizó la propiedad 9 a ''Alquilada'' por la firma del contrato C-20261028-0005.', 2, N'5', CAST(N'2026-10-28T22:03:04.217' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (648, N'45000111', N'Se registró una visita Pendiente del cliente 47307577 para la propiedad 14 el 28/10/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-10-28T22:03:51.917' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (649, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T12:09:47.187' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (650, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T12:12:28.127' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (651, N'45000111', N'Se registró el cliente Pedro Gomez (DNI 34567890).', 2, N'4', CAST(N'2026-11-02T12:13:01.800' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (652, N'45000111', N'Se modificó el cliente Pedro Gomez (DNI 34567890).', 2, N'4', CAST(N'2026-11-02T12:13:13.577' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (653, N'45000111', N'Se registró la propiedad 47 - Maipu.', 2, N'5', CAST(N'2026-11-02T12:14:41.030' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (654, N'45000111', N'Se registró una visita Pendiente del cliente 47307577 para la propiedad 14 el 02/11/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-11-02T12:15:18.820' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (655, N'45000111', N'Se actualizó el estado de la visita 33 a ''Realizada'' para el cliente 47307577.', 2, N'6', CAST(N'2026-11-02T12:16:21.933' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (656, N'45000111', N'Se generó el contrato C-20261102-0001 (propietario DNI 47307577, cliente DNI 45111000, propiedad 14).', 2, N'7', CAST(N'2026-11-02T12:17:14.797' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (657, N'47006530', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T12:18:48.720' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (658, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T17:09:53.733' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (659, N'45000111', N'Se generó el contrato C-20261102-0002 (propietario DNI 47307577, cliente DNI 45111000, propiedad 11).', 2, N'7', CAST(N'2026-11-02T17:10:38.243' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (660, N'45000111', N'Se actualizó el contrato C-20261102-0002 a ''Cancelado''. La propiedad 11 mantiene su disponibilidad.', 2, N'7', CAST(N'2026-11-02T17:11:51.280' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (661, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T17:54:47.987' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (662, N'45000111', N'Se generó el contrato C-20261102-0003 (propietario DNI 47307577, cliente DNI 45111000, propiedad 10).', 2, N'7', CAST(N'2026-11-02T18:02:14.690' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (663, N'45000111', N'Se actualizó el contrato C-20261102-0003 a ''Firmado'' (propietario DNI 47307577, cliente DNI 45111000).', 2, N'7', CAST(N'2026-11-02T18:02:32.950' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (664, N'45000111', N'Se actualizó la propiedad 10 a ''Vendida'' por la firma del contrato C-20261102-0003.', 2, N'5', CAST(N'2026-11-02T18:02:32.957' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (665, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T18:08:28.507' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (666, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-02T19:43:31.800' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (667, N'45000111', N'Se generó el contrato C-20261102-0004 (propietario DNI 47307577, cliente DNI 45111000, propiedad 52).', 2, N'7', CAST(N'2026-11-02T19:44:20.060' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (668, N'45000111', N'Se actualizó el contrato C-20261102-0004 a ''Firmado'' (propietario DNI 47307577, cliente DNI 45111000).', 2, N'7', CAST(N'2026-11-02T19:44:36.673' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (669, N'45000111', N'Se actualizó la propiedad 52 a ''Vendida'' por la firma del contrato C-20261102-0004.', 2, N'5', CAST(N'2026-11-02T19:44:36.673' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (670, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-03T10:38:29.467' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (671, N'45000111', N'Se generó el contrato C-20261103-0001 (propietario DNI 45111000, cliente DNI 47307577, propiedad 53).', 2, N'7', CAST(N'2026-11-03T10:39:09.443' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (672, N'45000111', N'Se actualizó el contrato C-20261103-0001 a ''Firmado'' (propietario DNI 45111000, cliente DNI 47307577).', 2, N'7', CAST(N'2026-11-03T10:39:28.767' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (673, N'45000111', N'Se actualizó la propiedad 53 a ''Alquilada'' por la firma del contrato C-20261103-0001.', 2, N'5', CAST(N'2026-11-03T10:39:28.773' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (674, N'45000111', N'Se registró una visita Pendiente del cliente 47307577 para la propiedad 47 el 03/11/2026 (10:00 - 11:00).', 2, N'6', CAST(N'2026-11-03T10:41:18.153' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (675, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-03T21:55:02.187' AS DateTime))
INSERT [dbo].[BitacoraEventos] ([Id], [DNI], [Evento], [Criticidad], [Modulo], [FechaHora]) VALUES (676, N'45000111', N'Realizo login exitoso.', 2, N'1', CAST(N'2026-11-03T21:55:46.050' AS DateTime))
SET IDENTITY_INSERT [dbo].[BitacoraEventos] OFF
GO

/* Cliente (14 fila(s)) */
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'12345678', N'Tomas', N'Calella', N'1144678966', N'tomascalella@gmail.com', 624675037217218697)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'23456789', N'Daniel', N'Babio', N'45678902', N'danibabio@gmail.com', 3635868282330350058)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'23678965', N'Melina', N'Esquivel', N'1167895674', N'meliesquivel@gmail.com', 7842354704182905220)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'29779483', N'Esteban', N'Ros', N'1121604600', N'eross@gmail.com', 5532726633881041000)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'34567890', N'Pedro', N'Gomez', N'1156783457', N'pedrogomez@gmail.com', 787975482440618635)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'34678912', N'Juan', N'Calella', N'11223347', N'juancalella@gmail.com', 2022068075478700307)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'45111000', N'Dolores', N'Noguera', N'1132335706', N'dolonoguera@gmail.com', 8454069221627799443)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'45178900', N'Benjamin', N'Aguila', N'34678907', N'benjacapo@gmail.com', 352922241227864336)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'46307577', N'Milagros', N'Araujo', N'1132335706', N'milagrosaraujo@gmail.com', 4397218846250546963)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'46948480', N'Alma', N'Castro Barrio', N'1123456789', N'almacastrobarrio@gmail.com', 7611869621120610048)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'47307577', N'Mili', N'Calella', N'67459078', N'milicalella@gmail.com', 1724000896757236346)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'47390789', N'Pilar', N'Moreno', N'78906754', N'pilinashe@gmail.com', 6367378474109868138)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'88765123', N'Rufina', N'Calella', N'45786532', N'rufinacalella@gmail.com', 6197183744928447395)
INSERT [dbo].[Cliente] ([DNI], [Nombre], [Apellido], [Telefono], [Email], [DVH]) VALUES (N'98765432', N'Benjamin', N'Calella', N'1156783456', N'benjacalella@gmail.com', 7860396187525531508)
GO

/* Contrato (10 fila(s)) */
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261028-0001', CAST(N'2026-10-28' AS Date), N'45111000', N'47307577', 14, N'Alquiler', CAST(N'2026-10-28' AS Date), CAST(N'2026-11-28' AS Date), CAST(90000000.00 AS Decimal(18, 2)), N'Contado', CAST(90000.00 AS Decimal(18, 2)), N'Firmado', 1279209226534761667)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261028-0002', CAST(N'2026-10-28' AS Date), N'45111000', N'47307577', 11, N'Venta', CAST(N'2026-10-28' AS Date), CAST(N'2026-11-28' AS Date), CAST(80900000.00 AS Decimal(18, 2)), N'Financiado', CAST(90000.00 AS Decimal(18, 2)), N'Firmado', 1037121644465467040)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261028-0003', CAST(N'2026-10-28' AS Date), N'45111000', N'47307577', 11, N'Venta', CAST(N'2026-10-28' AS Date), CAST(N'2026-11-28' AS Date), CAST(9000000.00 AS Decimal(18, 2)), N'Contado', CAST(9000.00 AS Decimal(18, 2)), N'Cancelado', 513979175088131947)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261028-0004', CAST(N'2026-10-28' AS Date), N'45111000', N'47307577', 11, N'Venta', CAST(N'2026-10-28' AS Date), CAST(N'2026-11-28' AS Date), CAST(900000.00 AS Decimal(18, 2)), N'Financiado', CAST(90000.00 AS Decimal(18, 2)), N'Pendiente', 6396739110314037297)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261028-0005', CAST(N'2026-10-28' AS Date), N'47307577', N'45111000', 9, N'Alquiler', CAST(N'2026-10-28' AS Date), CAST(N'2026-11-28' AS Date), CAST(890.00 AS Decimal(18, 2)), N'Financiado', CAST(90.00 AS Decimal(18, 2)), N'Firmado', 8575975194147167091)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261102-0001', CAST(N'2026-11-02' AS Date), N'47307577', N'45111000', 14, N'Alquiler', CAST(N'2026-11-02' AS Date), CAST(N'2026-12-02' AS Date), CAST(70000.00 AS Decimal(18, 2)), N'Contado', CAST(890.00 AS Decimal(18, 2)), N'Pendiente', 8003905569968501111)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261102-0002', CAST(N'2026-11-02' AS Date), N'47307577', N'45111000', 11, N'Alquiler', CAST(N'2026-11-02' AS Date), CAST(N'2026-12-02' AS Date), CAST(90.00 AS Decimal(18, 2)), N'Contado', CAST(9.00 AS Decimal(18, 2)), N'Cancelado', 6205856040375489843)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261102-0003', CAST(N'2026-11-02' AS Date), N'47307577', N'45111000', 10, N'Venta', CAST(N'2026-11-02' AS Date), CAST(N'2026-12-02' AS Date), CAST(90000.00 AS Decimal(18, 2)), N'Cuotas', CAST(900.00 AS Decimal(18, 2)), N'Firmado', 7108548558960454761)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261102-0004', CAST(N'2026-11-02' AS Date), N'47307577', N'45111000', 52, N'Venta', CAST(N'2026-11-02' AS Date), CAST(N'2026-12-02' AS Date), CAST(900000.00 AS Decimal(18, 2)), N'Contado', CAST(90000.00 AS Decimal(18, 2)), N'Firmado', 6428118899006839146)
INSERT [dbo].[Contrato] ([NroContrato], [FechaEmision], [DNIPropietario], [DNICliente], [CodigoPropiedad], [TipoContrato], [FechaInicio], [FechaFin], [MontoAcordado], [FormaPago], [DepositoGarantia], [Estado], [DVH]) VALUES (N'C-20261103-0001', CAST(N'2026-11-03' AS Date), N'45111000', N'47307577', 53, N'Alquiler', CAST(N'2026-11-03' AS Date), CAST(N'2026-12-03' AS Date), CAST(1800000.00 AS Decimal(18, 2)), N'Financiado', CAST(180000.00 AS Decimal(18, 2)), N'Firmado', 2080695240558240312)
GO

/* DigitoVerificador (5 fila(s)) */
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DVV], [DVH]) VALUES (N'Contrato', -7710083561709564633, 7746414830304721321)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DVV], [DVH]) VALUES (N'Familia', -934074675226458431, 2825178519962287263)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DVV], [DVH]) VALUES (N'Patente', 3763894548417655905, 5310278000647144596)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DVV], [DVH]) VALUES (N'Rol', 529858410120997921, 777510134062493653)
INSERT [dbo].[DigitoVerificador] ([NombreTabla], [DVV], [DVH]) VALUES (N'Usuario', -9023517110186959074, 6485132167241105880)
GO

/* Familia (4 fila(s)) */
SET IDENTITY_INSERT [dbo].[Familia] ON 
INSERT [dbo].[Familia] ([Id], [Nombre], [DVH]) VALUES (4, N'Usuario', 1609856342293148682)
INSERT [dbo].[Familia] ([Id], [Nombre], [DVH]) VALUES (5, N'SuperAdministrador', 7845255393029259478)
INSERT [dbo].[Familia] ([Id], [Nombre], [DVH]) VALUES (9, N'prueba', 3476683481434340646)
INSERT [dbo].[Familia] ([Id], [Nombre], [DVH]) VALUES (10, N'Agente Inmobiliario', 4580874181726344379)
SET IDENTITY_INSERT [dbo].[Familia] OFF
GO

/* Familia_Familia (2 fila(s)) */
INSERT [dbo].[Familia_Familia] ([IdFamiliaPadre], [IdFamiliaHija]) VALUES (5, 4)
INSERT [dbo].[Familia_Familia] ([IdFamiliaPadre], [IdFamiliaHija]) VALUES (9, 4)
GO

/* Familia_Patente (17 fila(s)) */
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (4, 1)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (4, 2)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (4, 3)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (4, 5)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (5, 4)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (5, 6)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (5, 7)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (5, 8)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (5, 9)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 1)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 2)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 3)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 5)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 10)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 11)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 12)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 13)
GO

/* Idioma (2 fila(s)) */
SET IDENTITY_INSERT [dbo].[Idioma] ON 
INSERT [dbo].[Idioma] ([Id], [Nombre]) VALUES (1, N'ESP')
INSERT [dbo].[Idioma] ([Id], [Nombre]) VALUES (2, N'ING')
SET IDENTITY_INSERT [dbo].[Idioma] OFF
GO

/* Patente (13 fila(s)) */
SET IDENTITY_INSERT [dbo].[Patente] ON 
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (1, N'Cambiar Clave', 2458530159571233800)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (2, N'Cambiar Idioma', 4910504425373317673)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (3, N'Cerrar Sesion', 6090868369147041441)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (4, N'Gestion Usuario', 1015917417283779837)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (5, N'Iniciar Sesion', 4751757804338581459)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (6, N'Auditoria Eventos', 5795750116311613863)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (7, N'Gestion Roles', 3239262242312090827)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (8, N'Gestion Familia', 4830395493474754246)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (9, N'Gestion Respaldo', 8086061736055250229)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (10, N'Gestion Clientes', 2346537011192952489)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (11, N'Seleccionar Propiedad', 8737598813736039242)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (12, N'Agendar Visita', 6304633519823353721)
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (13, N'Actualizar Estado Visita', 536309660926301926)
SET IDENTITY_INSERT [dbo].[Patente] OFF
GO

/* Propiedad (25 fila(s)) */
SET IDENTITY_INSERT [dbo].[Propiedad] ON 
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (9, N'Av. San Martín 450', N'Casa', N'Alquilada', CAST(135000.00 AS Decimal(18, 2)), CAST(120.00 AS Decimal(10, 2)), 4, 3, 2, 9046242376090539124)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (10, N'Calle 9 de Julio 820', N'Departamento', N'Vendida', CAST(75000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(10, 2)), 2, 1, 1, 7524375247900150426)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (11, N'Boulevard Haedo 300', N'Local', N'Vendida', CAST(90000.00 AS Decimal(18, 2)), CAST(75.00 AS Decimal(10, 2)), 1, 0, 1, 6970256127217298619)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (14, N'Int. Amenedo 148', N'Casa', N'Alquilada', CAST(3450000.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(10, 2)), 2, 2, 2, 6186952351168878170)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (47, N'Maipu', N'Casa', N'Disponible', CAST(780000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(10, 2)), 1, 1, 1, 6632829054450153209)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (48, N'Av. Belgrano 2450', N'Departamento', N'Disponible', CAST(98000.00 AS Decimal(18, 2)), CAST(62.50 AS Decimal(10, 2)), 3, 2, 1, 3510235778020754581)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (49, N'Calle Sarmiento 1180', N'Casa', N'Disponible', CAST(165000.00 AS Decimal(18, 2)), CAST(140.00 AS Decimal(10, 2)), 5, 3, 2, 4366710355551357379)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (50, N'Av. Colon 880', N'Local', N'Disponible', CAST(72000.00 AS Decimal(18, 2)), CAST(55.00 AS Decimal(10, 2)), 2, 0, 1, 6811071956749991052)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (51, N'Los Aromos 315', N'Casa', N'Disponible', CAST(210000.00 AS Decimal(18, 2)), CAST(180.00 AS Decimal(10, 2)), 6, 4, 3, 7218025471422077988)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (52, N'Av. Illia 1420', N'Departamento', N'Vendida', CAST(105000.00 AS Decimal(18, 2)), CAST(70.00 AS Decimal(10, 2)), 3, 2, 2, 3471486002766776564)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (53, N'Calle Rivadavia 2320', N'Local', N'Alquilada', CAST(88000.00 AS Decimal(18, 2)), CAST(65.00 AS Decimal(10, 2)), 2, 0, 1, 4559528927144703225)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (54, N'Barrio Los Pinos 12', N'Casa', N'Disponible', CAST(190000.00 AS Decimal(18, 2)), CAST(160.00 AS Decimal(10, 2)), 5, 3, 3, 1794888962016967616)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (55, N'Av. de Mayo 760', N'Departamento', N'Disponible', CAST(92000.00 AS Decimal(18, 2)), CAST(48.00 AS Decimal(10, 2)), 2, 1, 1, 4734959790666368270)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (56, N'Camino Rural Km 5', N'Terreno', N'Disponible', CAST(58000.00 AS Decimal(18, 2)), CAST(800.00 AS Decimal(10, 2)), 0, 0, 0, 1847217699179070291)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (57, N'Av. Roca 1990', N'Departamento', N'Disponible', CAST(118000.00 AS Decimal(18, 2)), CAST(75.00 AS Decimal(10, 2)), 4, 2, 2, 3171286530478149863)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (58, N'Calle Mitre 640', N'Local', N'Disponible', CAST(95000.00 AS Decimal(18, 2)), CAST(72.00 AS Decimal(10, 2)), 3, 0, 2, 5487312551806090080)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (59, N'Loteo El Sol 27', N'Terreno', N'Disponible', CAST(47000.00 AS Decimal(18, 2)), CAST(620.00 AS Decimal(10, 2)), 0, 0, 0, 237389661087232101)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (60, N'Av. Alvear 2210', N'Casa', N'Reservada', CAST(245000.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(10, 2)), 6, 4, 3, 8598732268299501778)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (61, N'Calle French 455', N'Departamento', N'Reservada', CAST(132000.00 AS Decimal(18, 2)), CAST(82.00 AS Decimal(10, 2)), 3, 2, 2, 1094809577628625791)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (62, N'Av. Pellegrini 1700', N'Local', N'Reservada', CAST(101000.00 AS Decimal(18, 2)), CAST(78.00 AS Decimal(10, 2)), 3, 0, 1, 5381294825143805444)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (63, N'Av. Gral. Paz 340', N'Casa', N'Vendida', CAST(198000.00 AS Decimal(18, 2)), CAST(170.00 AS Decimal(10, 2)), 5, 3, 2, 8916519808937389440)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (64, N'Calle Cordoba 1250', N'Departamento', N'Vendida', CAST(112000.00 AS Decimal(18, 2)), CAST(68.00 AS Decimal(10, 2)), 3, 2, 1, 6870173148983020071)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (65, N'Av. San Juan 980', N'Local', N'Vendida', CAST(89000.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(10, 2)), 2, 0, 1, 2879181635093198772)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (66, N'Av. Caseros 2150', N'Departamento', N'Alquilada', CAST(84000.00 AS Decimal(18, 2)), CAST(58.00 AS Decimal(10, 2)), 2, 1, 1, 7462691723533379539)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (67, N'Calle Salta 740', N'Local', N'Alquilada', CAST(76000.00 AS Decimal(18, 2)), CAST(52.00 AS Decimal(10, 2)), 2, 0, 1, 1844012713953421959)
SET IDENTITY_INSERT [dbo].[Propiedad] OFF
GO

/* Rol (4 fila(s)) */
SET IDENTITY_INSERT [dbo].[Rol] ON 
INSERT [dbo].[Rol] ([Id], [Nombre], [DVH]) VALUES (1, N'RolMal', 6950883660973347529)
INSERT [dbo].[Rol] ([Id], [Nombre], [DVH]) VALUES (2, N'Basico', 7106112860620502581)
INSERT [dbo].[Rol] ([Id], [Nombre], [DVH]) VALUES (3, N'Prueba', 3345124144804916479)
INSERT [dbo].[Rol] ([Id], [Nombre], [DVH]) VALUES (4, N'Agente Inmobiliario', 1574481817431782948)
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO

/* Rol_Familia (4 fila(s)) */
INSERT [dbo].[Rol_Familia] ([IdRol], [IdFamilia]) VALUES (1, 5)
INSERT [dbo].[Rol_Familia] ([IdRol], [IdFamilia]) VALUES (2, 4)
INSERT [dbo].[Rol_Familia] ([IdRol], [IdFamilia]) VALUES (3, 9)
INSERT [dbo].[Rol_Familia] ([IdRol], [IdFamilia]) VALUES (4, 10)
GO

/* Rol_Patente (1 fila(s)) */
INSERT [dbo].[Rol_Patente] ([IdRol], [IdPatente]) VALUES (3, 7)
GO

/* Usuario (5 fila(s)) */
INSERT [dbo].[Usuario] ([DNI], [Nombre], [Apellido], [Username], [Email], [PasswordHash], [Activo], [Bloqueo], [Intentos], [IdRol], [UltimoIntentoFallido], [IdIdioma], [DVH]) VALUES (N'21487343', N'Marisa', N'Rojas', N'marisa21487343', N'marisarojas@gmail.com', N'ff960cb55673958c594d0daaab1e368651c75c02f9687192a1811e7b180336a7', 1, 1, 4, 2, CAST(N'2026-05-19T16:45:05.357' AS DateTime), 1, 2755667542133604088)
INSERT [dbo].[Usuario] ([DNI], [Nombre], [Apellido], [Username], [Email], [PasswordHash], [Activo], [Bloqueo], [Intentos], [IdRol], [UltimoIntentoFallido], [IdIdioma], [DVH]) VALUES (N'45000111', N'Delfina ', N'Pardo', N'delfina45000111', N'delfipardo@gmail.com', N'3887e8d64a56a674b901fe558d9ce033f8a2980e7c765dff2e30f13ff66b3536', 1, 0, 0, 4, NULL, 1, 3316386785167454430)
INSERT [dbo].[Usuario] ([DNI], [Nombre], [Apellido], [Username], [Email], [PasswordHash], [Activo], [Bloqueo], [Intentos], [IdRol], [UltimoIntentoFallido], [IdIdioma], [DVH]) VALUES (N'45813074', N'Naiara', N'Esterman', N'naiara45813074', N'naiaraesterman63@gmail.com', N'e82cff6eaf002bca3627fa8a89617d585d5745792dd725cced9650609208265a', 1, 0, 0, 2, NULL, 1, 8930650879846902315)
INSERT [dbo].[Usuario] ([DNI], [Nombre], [Apellido], [Username], [Email], [PasswordHash], [Activo], [Bloqueo], [Intentos], [IdRol], [UltimoIntentoFallido], [IdIdioma], [DVH]) VALUES (N'47006530', N'Benjamin', N'Aguila', N'benjamin47006530', N'benjaaguila6@gmail.com', N'afd9fcfa114d0865303015b5cf57efe9f2088634dc553bd556bb3a2189362a46', 1, 0, 0, 1, NULL, 1, 8102442479945884455)
INSERT [dbo].[Usuario] ([DNI], [Nombre], [Apellido], [Username], [Email], [PasswordHash], [Activo], [Bloqueo], [Intentos], [IdRol], [UltimoIntentoFallido], [IdIdioma], [DVH]) VALUES (N'47307577', N'Milagros', N'Calella', N'milagros47307577', N'milagroscalella@gmail.com', N'67d1514f9bb64c1adcec4ff70c012a40d675612d4403bfae978193547b751142', 1, 0, 2, 1, CAST(N'2026-09-26T14:36:34.760' AS DateTime), 2, 4764823350138298870)
GO

/* Visita (33 fila(s)) */
SET IDENTITY_INSERT [dbo].[Visita] ON 
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (1, 9, N'31111222', CAST(N'2026-09-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 2170815536184747765, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (2, 9, N'28555666', CAST(N'2026-09-28' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), N'Realizada', 1149461385605462660, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (3, 9, N'32222111', CAST(N'2026-09-28' AS Date), CAST(N'14:00:00' AS Time), CAST(N'15:00:00' AS Time), N'Cancelada', 4138184366164131511, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (4, 9, N'45111000', CAST(N'2026-09-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 1007113002638449286, N'Muy buen lugar')
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (5, 11, N'45111000', CAST(N'2026-09-30' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 4955683060404294169, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (6, 9, N'12345678', CAST(N'2026-09-28' AS Date), CAST(N'11:00:00' AS Time), CAST(N'13:00:00' AS Time), N'Cancelada', 5069027444895130887, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (7, 9, N'45111000', CAST(N'2026-10-08' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Cancelada', 7753752394098239969, N'No le gusto')
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (8, 9, N'46307577', CAST(N'2026-09-30' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 7795496826577765485, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (9, 11, N'98765432', CAST(N'2026-10-09' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Cancelada', 6616915911092015351, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (10, 11, N'45111000', CAST(N'2026-09-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 4815146728675154126, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (11, 10, N'88765123', CAST(N'2026-10-09' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 6225082609907433081, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (12, 9, N'34678912', CAST(N'2026-10-07' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 3424342381514465665, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (13, 9, N'47390789', CAST(N'2026-10-08' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 4739315872195845256, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (14, 10, N'45111000', CAST(N'2026-10-11' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 1866421609674567290, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (15, 10, N'45111000', CAST(N'2026-10-31' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 2430742900994651805, N'Muy buen lugar')
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (16, 10, N'45111000', CAST(N'2026-11-05' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 5460772209921150659, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (17, 10, N'45111000', CAST(N'2026-10-22' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 5327089013623650665, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (18, 11, N'45111000', CAST(N'2026-10-26' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 8912769663940245255, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (19, 10, N'45111000', CAST(N'2026-12-17' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 3911748941084903672, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (20, 9, N'45111000', CAST(N'2026-10-20' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 7878349657500747856, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (21, 9, N'45111000', CAST(N'2026-10-04' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 3194611617362475660, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (22, 9, N'45111000', CAST(N'2026-10-10' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 8140262051879239875, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (23, 10, N'29779483', CAST(N'2026-11-18' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 8261781976372646270, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (25, 14, N'45111000', CAST(N'2026-10-10' AS Date), CAST(N'11:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Pendiente', 6735946772202899829, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (26, 10, N'47307577', CAST(N'2026-10-14' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 6218873620596585600, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (27, 14, N'45111000', CAST(N'2026-09-30' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 135408930023745238, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (28, 9, N'45111000', CAST(N'2026-10-30' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 6180987746321779010, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (29, 11, N'23456789', CAST(N'2026-09-30' AS Date), CAST(N'12:00:00' AS Time), CAST(N'13:00:00' AS Time), N'Pendiente', 3362062019244670404, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (30, 10, N'45111000', CAST(N'2027-01-30' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 7133101061682961078, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (31, 9, N'47307577', CAST(N'2026-10-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 4891771506573452122, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (32, 14, N'47307577', CAST(N'2026-10-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 7325502454002005005, NULL)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (33, 14, N'47307577', CAST(N'2026-11-02' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Realizada', 4293667704797148398, N'Muy buena')
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH], [Observaciones]) VALUES (34, 47, N'47307577', CAST(N'2026-11-03' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 4173607161543822933, NULL)
SET IDENTITY_INSERT [dbo].[Visita] OFF
GO


/* ============================================================================================
   3. INDICES UNICOS
   --------------------------------------------------------------------------------------------
   Se mantiene UN SOLO indice unico por Email en Cliente y Usuario.
   (La base original tenia 7 indices duplicados sobre Usuario.Email.)
   ============================================================================================ */

SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Cliente] ADD CONSTRAINT [UQ_Cliente_Email] UNIQUE NONCLUSTERED
(
    [Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [UQ_Usuario_Email] UNIQUE NONCLUSTERED
(
    [Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/* ============================================================================================
   4. DEFAULTS
   ============================================================================================ */

ALTER TABLE [dbo].[BitacoraEventos] ADD CONSTRAINT [DF_BitacoraEventos_FechaHora] DEFAULT (getdate()) FOR [FechaHora]
GO

ALTER TABLE [dbo].[Contrato] ADD CONSTRAINT [DF_Contrato_Estado] DEFAULT ('Pendiente') FOR [Estado]
GO

ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_Activo] DEFAULT ((1)) FOR [Activo]
GO

ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_Bloqueo] DEFAULT ((0)) FOR [Bloqueo]
GO

ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_Intentos] DEFAULT ((0)) FOR [Intentos]
GO

ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [DF_Usuario_IdIdioma] DEFAULT ((1)) FOR [IdIdioma]
GO

/* ============================================================================================
   5. CLAVES FORANEAS
   ============================================================================================ */

ALTER TABLE [dbo].[BitacoraEventos] WITH CHECK ADD CONSTRAINT [FK_BitacoraEventos_Usuario] FOREIGN KEY([DNI])
REFERENCES [dbo].[Usuario] ([DNI])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BitacoraEventos] CHECK CONSTRAINT [FK_BitacoraEventos_Usuario]
GO

ALTER TABLE [dbo].[Contrato] WITH CHECK ADD CONSTRAINT [FK_Contrato_Cliente] FOREIGN KEY([DNICliente])
REFERENCES [dbo].[Cliente] ([DNI])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK_Contrato_Cliente]
GO

ALTER TABLE [dbo].[Contrato] WITH CHECK ADD CONSTRAINT [FK_Contrato_Propiedad] FOREIGN KEY([CodigoPropiedad])
REFERENCES [dbo].[Propiedad] ([Id])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK_Contrato_Propiedad]
GO

ALTER TABLE [dbo].[Contrato] WITH CHECK ADD CONSTRAINT [FK_Contrato_Propietario] FOREIGN KEY([DNIPropietario])
REFERENCES [dbo].[Cliente] ([DNI])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK_Contrato_Propietario]
GO

ALTER TABLE [dbo].[Familia_Familia] WITH CHECK ADD CONSTRAINT [FK_Familia_FamiliaHija] FOREIGN KEY([IdFamiliaHija])
REFERENCES [dbo].[Familia] ([Id])
GO
ALTER TABLE [dbo].[Familia_Familia] CHECK CONSTRAINT [FK_Familia_FamiliaHija]
GO

ALTER TABLE [dbo].[Familia_Familia] WITH CHECK ADD CONSTRAINT [FK_Familia_FamiliaPadre] FOREIGN KEY([IdFamiliaPadre])
REFERENCES [dbo].[Familia] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Familia_Familia] CHECK CONSTRAINT [FK_Familia_FamiliaPadre]
GO

ALTER TABLE [dbo].[Familia_Patente] WITH CHECK ADD CONSTRAINT [FK_Familia_FamiliaPatente] FOREIGN KEY([IdFamilia])
REFERENCES [dbo].[Familia] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Familia_Patente] CHECK CONSTRAINT [FK_Familia_FamiliaPatente]
GO

ALTER TABLE [dbo].[Familia_Patente] WITH CHECK ADD CONSTRAINT [FK_Patente_FamiliaPatente] FOREIGN KEY([IdPatente])
REFERENCES [dbo].[Patente] ([Id])
GO
ALTER TABLE [dbo].[Familia_Patente] CHECK CONSTRAINT [FK_Patente_FamiliaPatente]
GO

ALTER TABLE [dbo].[Rol_Familia] WITH CHECK ADD CONSTRAINT [FK_Familia_RolFamilia] FOREIGN KEY([IdFamilia])
REFERENCES [dbo].[Familia] ([Id])
GO
ALTER TABLE [dbo].[Rol_Familia] CHECK CONSTRAINT [FK_Familia_RolFamilia]
GO

ALTER TABLE [dbo].[Rol_Familia] WITH CHECK ADD CONSTRAINT [FK_Rol_RolFamilia] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rol_Familia] CHECK CONSTRAINT [FK_Rol_RolFamilia]
GO

ALTER TABLE [dbo].[Rol_Patente] WITH CHECK ADD CONSTRAINT [FK_Patente_RolPatente] FOREIGN KEY([IdPatente])
REFERENCES [dbo].[Patente] ([Id])
GO
ALTER TABLE [dbo].[Rol_Patente] CHECK CONSTRAINT [FK_Patente_RolPatente]
GO

ALTER TABLE [dbo].[Rol_Patente] WITH CHECK ADD CONSTRAINT [FK_Rol_RolPatente] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rol_Patente] CHECK CONSTRAINT [FK_Rol_RolPatente]
GO

ALTER TABLE [dbo].[Usuario] WITH CHECK ADD CONSTRAINT [FK_Usuario_Idioma] FOREIGN KEY([IdIdioma])
REFERENCES [dbo].[Idioma] ([Id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Idioma]
GO

ALTER TABLE [dbo].[Usuario] WITH CHECK ADD CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([Id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

/* ============================================================================================
   6. CIERRE
   ============================================================================================ */

SET NOEXEC OFF
GO

USE [master]
GO

ALTER DATABASE [is--servicios] SET READ_WRITE
GO

PRINT N'Script de creacion de [is--servicios] finalizado.'
GO

/* ============================================================================================
   7. VERIFICACION (opcional, ejecutar a mano)
   --------------------------------------------------------------------------------------------
   Valores esperados en esta version del script:

     BitacoraEventos   539        Familia_Patente   17
     Cliente            14        Idioma             2
     Contrato           10        Patente           13
     DigitoVerificador   5        Propiedad         25
     Familia             4        Rol                4
     Familia_Familia     2        Rol_Familia        4
     Rol_Patente         1        Usuario            5
     Visita             33

     DVV Contrato = -7710083561709564633
   ============================================================================================ */

/*
USE [is--servicios]
GO

SELECT 'BitacoraEventos' AS Tabla, COUNT(*) AS Filas FROM [dbo].[BitacoraEventos]
UNION ALL SELECT 'Cliente',            COUNT(*) FROM [dbo].[Cliente]
UNION ALL SELECT 'Contrato',           COUNT(*) FROM [dbo].[Contrato]
UNION ALL SELECT 'DigitoVerificador',  COUNT(*) FROM [dbo].[DigitoVerificador]
UNION ALL SELECT 'Familia',            COUNT(*) FROM [dbo].[Familia]
UNION ALL SELECT 'Familia_Familia',    COUNT(*) FROM [dbo].[Familia_Familia]
UNION ALL SELECT 'Familia_Patente',    COUNT(*) FROM [dbo].[Familia_Patente]
UNION ALL SELECT 'Idioma',             COUNT(*) FROM [dbo].[Idioma]
UNION ALL SELECT 'Patente',            COUNT(*) FROM [dbo].[Patente]
UNION ALL SELECT 'Propiedad',          COUNT(*) FROM [dbo].[Propiedad]
UNION ALL SELECT 'Rol',                COUNT(*) FROM [dbo].[Rol]
UNION ALL SELECT 'Rol_Familia',        COUNT(*) FROM [dbo].[Rol_Familia]
UNION ALL SELECT 'Rol_Patente',        COUNT(*) FROM [dbo].[Rol_Patente]
UNION ALL SELECT 'Usuario',            COUNT(*) FROM [dbo].[Usuario]
UNION ALL SELECT 'Visita',             COUNT(*) FROM [dbo].[Visita]
ORDER BY Tabla;

SELECT [NombreTabla], [DVV] FROM [dbo].[DigitoVerificador] ORDER BY [NombreTabla];

SELECT COUNT(*) AS IndicesUnicosEmailUsuario
FROM sys.indexes
WHERE object_id = OBJECT_ID('dbo.Usuario') AND is_unique = 1 AND name LIKE 'UQ_%';
*/