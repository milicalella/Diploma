/*
	CUN02 - Seleccionar propiedad
	=============================
	Aplica sobre una base ya creada con scriptTerceraEntrega.sql.

	1) Crea la tabla Propiedad (catálogo).
	2) Carga el catálogo inicial de propiedades de ejemplo (con DVH).
*/

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

SET IDENTITY_INSERT [dbo].[Propiedad] ON 
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (1, N'Av. Corrientes 1234', N'Departamento', N'Disponible', 85000.00, 45.50, 2, 2, 1, 6891670729228791861)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (2, N'Av. del Libertador 567', N'Departamento', N'Disponible', 95000.00, 60.00, 3, 2, 1, 3436668636375701443)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (3, N'Calle Mitre 890', N'Casa', N'Disponible', 120000.00, 90.25, 3, 3, 2, 8333509121154202587)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (4, N'Av. Rivadavia 4321', N'Local', N'Alquilada', 60000.00, 80.00, 2, 0, 1, 4460235362475448320)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (5, N'Ruta 8 Km 42', N'Terreno', N'Disponible', 45000.00, 500.00, 0, 0, 0, 2443139282230703802)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (6, N'Av. Santa Fe 2150', N'Departamento', N'Vendida', 110000.00, 72.00, 3, 2, 2, 558608779194180994)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (7, N'Calle Belgrano 1500', N'Casa', N'Reservada', 145000.00, 110.00, 4, 3, 2, 129561837540272835)
INSERT [dbo].[Propiedad] ([Id], [Direccion], [Tipo], [Estado], [Precio], [SuperficieM2], [Ambientes], [Dormitorios], [Banios], [DVH]) VALUES (8, N'Av. Cabildo 3100', N'Local', N'Disponible', 78000.00, 65.00, 2, 0, 1, 3025600301518053041)
SET IDENTITY_INSERT [dbo].[Propiedad] OFF
GO