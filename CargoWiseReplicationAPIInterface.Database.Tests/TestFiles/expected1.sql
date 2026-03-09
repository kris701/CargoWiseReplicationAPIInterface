IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'CWO')
	EXEC('CREATE SCHEMA [CWO]')
GO

DROP PROCEDURE IF EXISTS [CWO].[MergeGlbGroup]
GO
IF type_id('[CWO].[GlbGroup_TVP]') IS NOT NULL
	DROP TYPE [CWO].[GlbGroup_TVP];
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'GlbGroup' AND xtype='U')
	CREATE TABLE [CWO].[GlbGroup] (
		[GG_PK] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_GlbGroup] PRIMARY KEY (GG_PK),
		[GG_Desc] NVARCHAR(MAX) NULL,
		[GG_ActiveDirectoryObjectGuid] UNIQUEIDENTIFIER NULL,
		[GG_IsSales] BIT NULL,
		[GG_SystemCreateTimeUtc] DATETIME NULL,
		[GG_SystemCreateUser] NVARCHAR(MAX) NULL,
		[GG_SystemLastEditTimeUtc] DATETIME NULL,
		[GG_SystemLastEditUser] NVARCHAR(MAX) NULL,
		[GG_IsSystemDefined] BIT NULL,
		[GG_IsActive] BIT NULL,
		[GG_IsValid] BIT NULL,
		[GG_Code] NVARCHAR(MAX) NULL,
		[GG_IsSecurityEnabled] BIT NULL,
		[GG_GC] UNIQUEIDENTIFIER NULL,
		[GG_DomainName] NVARCHAR(MAX) NULL,
		[GG_Category] NVARCHAR(MAX) NULL,
		[GG_GG_ParentGroup] UNIQUEIDENTIFIER NULL,
		[GG_ExternalId] NVARCHAR(MAX) NULL,
		[GG_Type] NVARCHAR(MAX) NULL,
		[GG_SystemCreateBranch] NVARCHAR(MAX) NULL,
		[GG_SystemCreateDepartment] NVARCHAR(MAX) NULL,
		[GG_CreatedAt] DATETIME NOT NULL,
		[GG_UpdatedAt] DATETIME NULL,
		[GG_UploadType] NVARCHAR(3) NOT NULL
	)
GO

CREATE TYPE [CWO].[GlbGroup_TVP] AS TABLE (
	[GG_PK] UNIQUEIDENTIFIER NOT NULL,
	[GG_Desc] NVARCHAR(MAX) NULL,
	[GG_ActiveDirectoryObjectGuid] UNIQUEIDENTIFIER NULL,
	[GG_IsSales] BIT NULL,
	[GG_SystemCreateTimeUtc] DATETIME NULL,
	[GG_SystemCreateUser] NVARCHAR(MAX) NULL,
	[GG_SystemLastEditTimeUtc] DATETIME NULL,
	[GG_SystemLastEditUser] NVARCHAR(MAX) NULL,
	[GG_IsSystemDefined] BIT NULL,
	[GG_IsActive] BIT NULL,
	[GG_IsValid] BIT NULL,
	[GG_Code] NVARCHAR(MAX) NULL,
	[GG_IsSecurityEnabled] BIT NULL,
	[GG_GC] UNIQUEIDENTIFIER NULL,
	[GG_DomainName] NVARCHAR(MAX) NULL,
	[GG_Category] NVARCHAR(MAX) NULL,
	[GG_GG_ParentGroup] UNIQUEIDENTIFIER NULL,
	[GG_ExternalId] NVARCHAR(MAX) NULL,
	[GG_Type] NVARCHAR(MAX) NULL,
	[GG_SystemCreateBranch] NVARCHAR(MAX) NULL,
	[GG_SystemCreateDepartment] NVARCHAR(MAX) NULL,
	[GG_UpdatedAt] DATETIME NULL,
	[OpCode] INT NOT NULL
)
GO

CREATE PROCEDURE [CWO].[MergeGlbGroup]
	 @Items [CWO].[GlbGroup_TVP] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRANSACTION;
		MERGE INTO [CWO].[GlbGroup] AS T
		USING @Items AS S ON T.GG_PK = S.GG_PK
		WHEN MATCHED AND S.OpCode = 1 THEN DELETE
		WHEN MATCHED AND S.OpCode IN (3,4) THEN UPDATE SET
			T.GG_PK = S.GG_PK,
			T.GG_Desc = S.GG_Desc,
			T.GG_ActiveDirectoryObjectGuid = S.GG_ActiveDirectoryObjectGuid,
			T.GG_IsSales = S.GG_IsSales,
			T.GG_SystemCreateTimeUtc = S.GG_SystemCreateTimeUtc,
			T.GG_SystemCreateUser = S.GG_SystemCreateUser,
			T.GG_SystemLastEditTimeUtc = S.GG_SystemLastEditTimeUtc,
			T.GG_SystemLastEditUser = S.GG_SystemLastEditUser,
			T.GG_IsSystemDefined = S.GG_IsSystemDefined,
			T.GG_IsActive = S.GG_IsActive,
			T.GG_IsValid = S.GG_IsValid,
			T.GG_Code = S.GG_Code,
			T.GG_IsSecurityEnabled = S.GG_IsSecurityEnabled,
			T.GG_GC = S.GG_GC,
			T.GG_DomainName = S.GG_DomainName,
			T.GG_Category = S.GG_Category,
			T.GG_GG_ParentGroup = S.GG_GG_ParentGroup,
			T.GG_ExternalId = S.GG_ExternalId,
			T.GG_Type = S.GG_Type,
			T.GG_SystemCreateBranch = S.GG_SystemCreateBranch,
			T.GG_SystemCreateDepartment = S.GG_SystemCreateDepartment,
			T.GG_UpdatedAt = GETUTCDATE()
		WHEN NOT MATCHED BY TARGET AND S.OpCode IN (2,3,4) THEN INSERT (
			GG_PK,
			GG_Desc,
			GG_ActiveDirectoryObjectGuid,
			GG_IsSales,
			GG_SystemCreateTimeUtc,
			GG_SystemCreateUser,
			GG_SystemLastEditTimeUtc,
			GG_SystemLastEditUser,
			GG_IsSystemDefined,
			GG_IsActive,
			GG_IsValid,
			GG_Code,
			GG_IsSecurityEnabled,
			GG_GC,
			GG_DomainName,
			GG_Category,
			GG_GG_ParentGroup,
			GG_ExternalId,
			GG_Type,
			GG_SystemCreateBranch,
			GG_SystemCreateDepartment,
			GG_CreatedAt,
			GG_UpdatedAt,
			GG_UploadType
		) VALUES (
			S.GG_PK,
			S.GG_Desc,
			S.GG_ActiveDirectoryObjectGuid,
			S.GG_IsSales,
			S.GG_SystemCreateTimeUtc,
			S.GG_SystemCreateUser,
			S.GG_SystemLastEditTimeUtc,
			S.GG_SystemLastEditUser,
			S.GG_IsSystemDefined,
			S.GG_IsActive,
			S.GG_IsValid,
			S.GG_Code,
			S.GG_IsSecurityEnabled,
			S.GG_GC,
			S.GG_DomainName,
			S.GG_Category,
			S.GG_GG_ParentGroup,
			S.GG_ExternalId,
			S.GG_Type,
			S.GG_SystemCreateBranch,
			S.GG_SystemCreateDepartment,
			GETUTCDATE(),
			NULL,
			'REP'
		);
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK;
		THROW;
	END CATCH
END
GO

