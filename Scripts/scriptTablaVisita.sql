/*
	CUN03 - Consultar disponibilidad (Agenda de visitas)
	=====================================================
	Aplica sobre una base ya creada con scriptTerceraEntrega.sql.

	1) Crea la tabla Visita (agenda de visitas/reservas).
	2) Carga el catálogo inicial de visitas de ejemplo (con DVH).

	Estado: Pendiente / Realizada / Cancelada.
	Regla de negocio: los turnos Pendiente y Realizada ocupan la agenda
	(Cancelada se ignora al validar superposiciones).
*/

CREATE TABLE [dbo].[Visita](
	[IdVisita] [int] IDENTITY(1,1) NOT NULL,
	[IdPropiedad] [int] NOT NULL,
	[DNICliente] [varchar](8) NOT NULL,
	[Fecha] [date] NOT NULL,
	[HoraInicio] [time](0) NOT NULL,
	[HoraFin] [time](0) NOT NULL,
	[Estado] [varchar](15) NOT NULL,
	[DVH] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVisita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Visita] ON 
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH]) VALUES (1, 1, N'31111222', CAST(N'2026-09-28' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), N'Pendiente', 6428128597858150670)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH]) VALUES (2, 1, N'28555666', CAST(N'2026-09-28' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), N'Realizada', 1149461385605462660)
INSERT [dbo].[Visita] ([IdVisita], [IdPropiedad], [DNICliente], [Fecha], [HoraInicio], [HoraFin], [Estado], [DVH]) VALUES (3, 2, N'32222111', CAST(N'2026-09-28' AS Date), CAST(N'14:00:00' AS Time), CAST(N'15:00:00' AS Time), N'Cancelada', 4138184366164131511)
SET IDENTITY_INSERT [dbo].[Visita] OFF
GO