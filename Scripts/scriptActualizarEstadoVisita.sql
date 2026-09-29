IF COL_LENGTH('dbo.Visita', 'Observaciones') IS NULL
BEGIN
    ALTER TABLE [dbo].[Visita] ADD [Observaciones] [varchar](500) NULL;
END
GO