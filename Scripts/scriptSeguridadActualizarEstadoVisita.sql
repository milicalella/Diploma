/*
	Seguridad - CUN05 Actualizar Estado de Visita
	==============================================
	Aplica sobre una base ya creada (tras correr los deltas de seguridad previos).

	1) Crea la patente 'Actualizar Estado Visita' (Id 13).
	2) La asigna a la familia 'Agente Inmobiliario' (acceso solo para ese rol).
	3) Recalcula DVV/DVH de la tabla Patente para que el Digito Verificador
	   del login no marque inconsistencias.
*/

IF NOT EXISTS (SELECT 1 FROM [dbo].[Patente] WHERE [Id] = 13)
BEGIN
    SET IDENTITY_INSERT [dbo].[Patente] ON
    INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (13, N'Actualizar Estado Visita', 536309660926301926)
    SET IDENTITY_INSERT [dbo].[Patente] OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Familia_Patente] WHERE [IdFamilia] = 10 AND [IdPatente] = 13)
BEGIN
    INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 13)
END
GO

UPDATE [dbo].[DigitoVerificador] SET [DVV] = 3763894548417655905, [DVH] = 5310278000647144596 WHERE [NombreTabla] = N'Patente'
GO