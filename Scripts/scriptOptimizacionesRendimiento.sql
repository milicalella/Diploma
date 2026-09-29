-- =============================================================
-- Optimizaciones de rendimiento (login y consultas frecuentes)
-- Ejecutar contra la base "is--servicios"
-- Todos los índices son idempotentes (IF NOT EXISTS).
-- =============================================================
SET NOCOUNT ON;

-- -------------------------------------------------------------
-- 1) Usuario: el login filtra por Username (tabla escaneada).
--    El dump creó además 3 veces el mismo índice único de Email
--    (ver sección 6 para detectarlos/depurarlos).
-- -------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Usuario_Username' AND object_id = OBJECT_ID(N'dbo.Usuario'))
    CREATE UNIQUE NONCLUSTERED INDEX UX_Usuario_Username ON dbo.Usuario(Username ASC);
GO

-- 2) Usuario.IdRol: joins y conteos de usuarios por rol.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Usuario_IdRol' AND object_id = OBJECT_ID(N'dbo.Usuario'))
    CREATE NONCLUSTERED INDEX IX_Usuario_IdRol ON dbo.Usuario(IdRol ASC);
GO

-- 3) Visita: disponibilidad por propiedad + fecha (agenda del día).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Visita_IdPropiedad_Fecha' AND object_id = OBJECT_ID(N'dbo.Visita'))
    CREATE NONCLUSTERED INDEX IX_Visita_IdPropiedad_Fecha
        ON dbo.Visita(IdPropiedad ASC, Fecha ASC)
        INCLUDE (HoraInicio, HoraFin, Estado, Observaciones, DVH);
GO

-- 4) Visita: historial por cliente.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Visita_DNICliente_Fecha' AND object_id = OBJECT_ID(N'dbo.Visita'))
    CREATE NONCLUSTERED INDEX IX_Visita_DNICliente_Fecha ON dbo.Visita(DNICliente ASC, Fecha ASC);
GO

-- 5) BitacoraEventos: filtros de Auditoría (rango de fechas + módulo).
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BitacoraEventos_FechaHora_Modulo' AND object_id = OBJECT_ID(N'dbo.BitacoraEventos'))
    CREATE NONCLUSTERED INDEX IX_BitacoraEventos_FechaHora_Modulo
        ON dbo.BitacoraEventos(FechaHora ASC, Modulo ASC)
        INCLUDE (DNI, Evento, Criticidad);
GO

-- 6) Tablas de relaciones (árbol Composite): búsquedas por rol/familia.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Rol_Familia_IdRol' AND object_id = OBJECT_ID(N'dbo.Rol_Familia'))
    CREATE NONCLUSTERED INDEX IX_Rol_Familia_IdRol ON dbo.Rol_Familia(IdRol ASC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Rol_Patente_IdRol' AND object_id = OBJECT_ID(N'dbo.Rol_Patente'))
    CREATE NONCLUSTERED INDEX IX_Rol_Patente_IdRol ON dbo.Rol_Patente(IdRol ASC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Familia_Patente_IdFamilia' AND object_id = OBJECT_ID(N'dbo.Familia_Patente'))
    CREATE NONCLUSTERED INDEX IX_Familia_Patente_IdFamilia ON dbo.Familia_Patente(IdFamilia ASC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Familia_Familia_IdPadre' AND object_id = OBJECT_ID(N'dbo.Familia_Familia'))
    CREATE NONCLUSTERED INDEX IX_Familia_Familia_IdPadre ON dbo.Familia_Familia(IdFamiliaPadre ASC);
GO

-- -------------------------------------------------------------
-- DIAGNÓSTICO (no modifica datos):
-- Índices actuales de Usuario. El dump creó la restricción UNIQUE
-- de Email hasta 3 veces; si aparecen varias con la misma columna,
-- conservar una y eliminar las restantes:
--   DROP INDEX <nombre_duplicado> ON dbo.Usuario;
-- -------------------------------------------------------------
SELECT i.name AS Indice,
       LTRIM(RTRIM(REPLACE(REPLACE(ic.COLUMNAS, ',', ', '), '[', ''))) AS Columnas
FROM sys.indexes i
CROSS APPLY (
    SELECT STUFF((
        SELECT ',' + QUOTENAME(c.name)
        FROM sys.index_columns ic2
        JOIN sys.columns c ON c.object_id = ic2.object_id AND c.column_id = ic2.column_id
        WHERE ic2.object_id = i.object_id AND ic2.index_id = i.index_id
        ORDER BY ic2.key_ordinal
        FOR XML PATH('')), 1, 1, '')
) ic(COLUMNAS)
WHERE i.object_id = OBJECT_ID(N'dbo.Usuario')
ORDER BY CASE WHEN i.index_id = 1 THEN 0 ELSE 1 END, i.name;
GO

PRINT 'Optimizaciones aplicadas.';