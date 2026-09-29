/*
	Seguridad - Caso de uso Registrar Cliente
	==========================================
	Aplica sobre una base ya creada con scriptTerceraEntrega.sql.

	1) Crea la patente 'Gestion Clientes'.
	2) Crea la familia 'Agente Inmobiliario' (con los permisos base + la nueva patente).
	3) Crea el rol 'Agente Inmobiliario' y lo asocia a la familia.
	4) Recalcula los DVV/DVH de las tablas Patente, Familia y Rol para que el
	   Digito Verificador del login no marque inconsistencias.
*/

SET IDENTITY_INSERT [dbo].[Patente] ON
INSERT [dbo].[Patente] ([Id], [Nombre], [DVH]) VALUES (10, N'Gestion Clientes', 2346537011192952489)
SET IDENTITY_INSERT [dbo].[Patente] OFF
GO

SET IDENTITY_INSERT [dbo].[Familia] ON
INSERT [dbo].[Familia] ([Id], [Nombre], [DVH]) VALUES (10, N'Agente Inmobiliario', 4580874181726344379)
SET IDENTITY_INSERT [dbo].[Familia] OFF
GO

INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 1)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 2)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 3)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 5)
INSERT [dbo].[Familia_Patente] ([IdFamilia], [IdPatente]) VALUES (10, 10)
GO

SET IDENTITY_INSERT [dbo].[Rol] ON
INSERT [dbo].[Rol] ([Id], [Nombre], [DVH]) VALUES (4, N'Agente Inmobiliario', 1574481817431782948)
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO

INSERT [dbo].[Rol_Familia] ([IdRol], [IdFamilia]) VALUES (4, 10)
GO

UPDATE [dbo].[DigitoVerificador] SET [DVV] = -934074675226458431, [DVH] = 2825178519962287263 WHERE [NombreTabla] = N'Familia'
UPDATE [dbo].[DigitoVerificador] SET [DVV] = 6632096627641512632, [DVH] = 4688140288695313689 WHERE [NombreTabla] = N'Patente'
UPDATE [dbo].[DigitoVerificador] SET [DVV] = 529858410120997921, [DVH] = 777510134062493653 WHERE [NombreTabla] = N'Rol'
GO