BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701070700_LayoutConfig_Table'
)
BEGIN
    CREATE TABLE [CBSAP].[LayoutConfig] (
        [LayoutConfigId] bigint NOT NULL IDENTITY,
        [Username] NVARCHAR(150) NULL,
        [LayoutValue] int NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_LayoutConfig] PRIMARY KEY ([LayoutConfigId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260701070700_LayoutConfig_Table'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260701070700_LayoutConfig_Table', N'9.0.4');
END;

COMMIT;
GO

