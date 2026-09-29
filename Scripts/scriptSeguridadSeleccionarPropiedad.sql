/*
	Seguridad - CUN02 Seleccionar propiedad
	=======================================
	Aplica sobre una base ya creada (tras correr scriptSeguridadAgenteInmobiliario.sql).

	1) Crea la patente 'Seleccionar Propiedad'.
	2) La asigna a la familia 'Agente Inmobiliario' (acceso solo para ese rol).
	3) Recalcula DVV/DVH de la tabla Patente para que el Digito Verificador
	   del login no marque inconsistencias.
*/

SET IDENTITY_INSERT [dbo].[Patente] ON
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (11, N'Seleccionar Propiedad', 8737598813736039242)
SET IDENTITY_INSERT [dbo].[Patente] OFF
GO

INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 11)
GO

UPDATE [dbo].[DigitoVerificador] SET [DVV] = -3077048632331999742, [DVH] = 2778051478874571574 WHERE [NombreTabla] = N'Patente'
GO