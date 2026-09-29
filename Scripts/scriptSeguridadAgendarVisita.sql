/*
	Seguridad - CUN03 Agendar Visita
	=================================
	Aplica sobre una base ya creada (tras correr scriptSeguridadAgenteInmobiliario.sql
	y scriptSeguridadSeleccionarPropiedad.sql).

	1) Crea la patente 'Agendar Visita'.
	2) La asigna a la familia 'Agente Inmobiliario' (acceso solo para ese rol).
	3) Recalcula DVV/DVH de la tabla Patente para que el Digito Verificador
	   del login no marque inconsistencias.
*/

SET IDENTITY_INSERT [dbo].[Patente] ON
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (12, N'Agendar Visita', 6304633519823353721)
SET IDENTITY_INSERT [dbo].[Patente] OFF
GO

INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 12)
GO

UPDATE [dbo].[DigitoVerificador] SET [DVV] = 3227584887491353979, [DVH] = 2465085895632608315 WHERE [NombreTabla] = N'Patente'
GO