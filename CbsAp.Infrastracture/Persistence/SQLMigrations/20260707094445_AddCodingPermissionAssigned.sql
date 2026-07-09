BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260707050611_20260707094445_AddCodingPermissionAssigned'
)
BEGIN
    CREATE TABLE [CBSAP].[CodingPermissionAssigned] (
        [ID] bigint NOT NULL IDENTITY,
        [NameCode] NVARCHAR NULL,
        [EntityProfileID] bigint NULL,
        [Category] NVARCHAR NULL,
        [IsAssigned] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(max) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_CodingPermissionAssigned] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260707050611_20260707094445_AddCodingPermissionAssigned'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260707050611_20260707094445_AddCodingPermissionAssigned', N'9.0.4');
END;

COMMIT;
GO

