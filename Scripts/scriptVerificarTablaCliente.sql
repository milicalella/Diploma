-- Verificacion: estructura real de las tablas usadas en el alta de cliente
-- Ejecutar contra la base is--servicios (la misma de App.config).
-- La estructura esperada es la del dump scriptTerceraEntrega.sql incluido en el repo.

SELECT 'BD destino' AS Seccion,
       DB_NAME() AS BaseDeDatos,
       SERVERPROPERTY('InstanceName') AS InstanciaSQL;

SELECT
    OBJECT_NAME(c.object_id) AS Tabla,
    c.name AS Columna,
    t.name AS Tipo,
    c.max_length AS LongitudMaxima,
    c.is_nullable AS AdmiteNull,
    c.is_identity AS EsIdentidad
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE OBJECT_NAME(c.object_id) IN ('Cliente', 'BitacoraEventos')
ORDER BY OBJECT_NAME(c.object_id), c.column_id;

-- Estructura esperada para [Cliente]:
--   DNI      varchar(50)   NOT NULL  (Primary Key)
--   Nombre   varchar(50)   NOT NULL
--   Apellido varchar(50)   NOT NULL
--   Telefono varchar(50)   NOT NULL
--   Email    varchar(100)  NOT NULL
--   DVH      bigint        NULL

-- Si la tabla Cliente de tu BD no tiene DVH (o difiere en alguna columna),
-- aplica el siguiente ALTER para alinearla con la estructura esperada:
--
-- IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cliente') AND name = 'DVH')
--     ALTER TABLE Cliente ADD DVH bigint NULL;