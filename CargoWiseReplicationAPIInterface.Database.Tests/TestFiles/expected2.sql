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

DROP PROCEDURE IF EXISTS [CWO].[MergeGlbCompany]
GO
IF type_id('[CWO].[GlbCompany_TVP]') IS NOT NULL
	DROP TYPE [CWO].[GlbCompany_TVP];
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'GlbCompany' AND xtype='U')
	CREATE TABLE [CWO].[GlbCompany] (
		[GC_PK] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_GlbCompany] PRIMARY KEY (GC_PK),
		[GC_Code] NVARCHAR(MAX) NULL,
		[GC_Name] NVARCHAR(MAX) NULL,
		[GC_BusinessRegNo] NVARCHAR(MAX) NULL,
		[GC_BusinessRegNo2] NVARCHAR(MAX) NULL,
		[GC_CustomsRegistrationNo] NVARCHAR(MAX) NULL,
		[GC_Address1] NVARCHAR(MAX) NULL,
		[GC_Address2] NVARCHAR(MAX) NULL,
		[GC_City] NVARCHAR(MAX) NULL,
		[GC_Phone] NVARCHAR(MAX) NULL,
		[GC_PostCode] NVARCHAR(MAX) NULL,
		[GC_State] NVARCHAR(MAX) NULL,
		[GC_Fax] NVARCHAR(MAX) NULL,
		[GC_WebAddress] NVARCHAR(MAX) NULL,
		[GC_NoOfAccountingPeriods] INT NULL,
		[GC_StartDate] DATETIME NULL,
		[GC_OH_OrgProxy] UNIQUEIDENTIFIER NULL,
		[GC_RX_NKLocalCurrency] NVARCHAR(MAX) NULL,
		[GC_RN_NKCountryCode] DECIMAL(19,4) NULL,
		[GC_LocalDocLanguage] DECIMAL(19,4) NULL,
		[GC_IsActive] BIT NULL,
		[GC_IsReciprocal] BIT NULL,
		[GC_IsGSTRegistered] BIT NULL,
		[GC_IsGSTCashBasis] BIT NULL,
		[GC_IsWHTRegistered] BIT NULL,
		[GC_IsWHTCashBasis] BIT NULL,
		[GC_IsValid] BIT NULL,
		[GC_Email] NVARCHAR(MAX) NULL,
		[GC_AddressMap] NVARCHAR(MAX) NULL,
		[GC_ValidationStatus] NVARCHAR(MAX) NULL,
		[GC_SystemCreateTimeUtc] DATETIME NULL,
		[GC_SystemCreateUser] NVARCHAR(MAX) NULL,
		[GC_SystemLastEditTimeUtc] DATETIME NULL,
		[GC_SystemLastEditUser] NVARCHAR(MAX) NULL
	)
GO

CREATE TYPE [CWO].[GlbCompany_TVP] AS TABLE (
	[GC_PK] UNIQUEIDENTIFIER NOT NULL,
	[GC_Code] NVARCHAR(MAX) NULL,
	[GC_Name] NVARCHAR(MAX) NULL,
	[GC_BusinessRegNo] NVARCHAR(MAX) NULL,
	[GC_BusinessRegNo2] NVARCHAR(MAX) NULL,
	[GC_CustomsRegistrationNo] NVARCHAR(MAX) NULL,
	[GC_Address1] NVARCHAR(MAX) NULL,
	[GC_Address2] NVARCHAR(MAX) NULL,
	[GC_City] NVARCHAR(MAX) NULL,
	[GC_Phone] NVARCHAR(MAX) NULL,
	[GC_PostCode] NVARCHAR(MAX) NULL,
	[GC_State] NVARCHAR(MAX) NULL,
	[GC_Fax] NVARCHAR(MAX) NULL,
	[GC_WebAddress] NVARCHAR(MAX) NULL,
	[GC_NoOfAccountingPeriods] INT NULL,
	[GC_StartDate] DATETIME NULL,
	[GC_OH_OrgProxy] UNIQUEIDENTIFIER NULL,
	[GC_RX_NKLocalCurrency] NVARCHAR(MAX) NULL,
	[GC_RN_NKCountryCode] DECIMAL(19,4) NULL,
	[GC_LocalDocLanguage] DECIMAL(19,4) NULL,
	[GC_IsActive] BIT NULL,
	[GC_IsReciprocal] BIT NULL,
	[GC_IsGSTRegistered] BIT NULL,
	[GC_IsGSTCashBasis] BIT NULL,
	[GC_IsWHTRegistered] BIT NULL,
	[GC_IsWHTCashBasis] BIT NULL,
	[GC_IsValid] BIT NULL,
	[GC_Email] NVARCHAR(MAX) NULL,
	[GC_AddressMap] NVARCHAR(MAX) NULL,
	[GC_ValidationStatus] NVARCHAR(MAX) NULL,
	[GC_SystemCreateTimeUtc] DATETIME NULL,
	[GC_SystemCreateUser] NVARCHAR(MAX) NULL,
	[GC_SystemLastEditTimeUtc] DATETIME NULL,
	[GC_SystemLastEditUser] NVARCHAR(MAX) NULL,
	[OpCode] INT NOT NULL
)
GO

CREATE PROCEDURE [CWO].[MergeGlbCompany]
	 @Items [CWO].[GlbCompany_TVP] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRANSACTION;
		MERGE INTO [CWO].[GlbCompany] AS T
		USING @Items AS S ON T.GC_PK = S.GC_PK
		WHEN MATCHED AND S.OpCode = 1 THEN DELETE
		WHEN MATCHED AND S.OpCode IN (3,4) THEN UPDATE SET
			T.GC_PK = S.GC_PK,
			T.GC_Code = S.GC_Code,
			T.GC_Name = S.GC_Name,
			T.GC_BusinessRegNo = S.GC_BusinessRegNo,
			T.GC_BusinessRegNo2 = S.GC_BusinessRegNo2,
			T.GC_CustomsRegistrationNo = S.GC_CustomsRegistrationNo,
			T.GC_Address1 = S.GC_Address1,
			T.GC_Address2 = S.GC_Address2,
			T.GC_City = S.GC_City,
			T.GC_Phone = S.GC_Phone,
			T.GC_PostCode = S.GC_PostCode,
			T.GC_State = S.GC_State,
			T.GC_Fax = S.GC_Fax,
			T.GC_WebAddress = S.GC_WebAddress,
			T.GC_NoOfAccountingPeriods = S.GC_NoOfAccountingPeriods,
			T.GC_StartDate = S.GC_StartDate,
			T.GC_OH_OrgProxy = S.GC_OH_OrgProxy,
			T.GC_RX_NKLocalCurrency = S.GC_RX_NKLocalCurrency,
			T.GC_RN_NKCountryCode = S.GC_RN_NKCountryCode,
			T.GC_LocalDocLanguage = S.GC_LocalDocLanguage,
			T.GC_IsActive = S.GC_IsActive,
			T.GC_IsReciprocal = S.GC_IsReciprocal,
			T.GC_IsGSTRegistered = S.GC_IsGSTRegistered,
			T.GC_IsGSTCashBasis = S.GC_IsGSTCashBasis,
			T.GC_IsWHTRegistered = S.GC_IsWHTRegistered,
			T.GC_IsWHTCashBasis = S.GC_IsWHTCashBasis,
			T.GC_IsValid = S.GC_IsValid,
			T.GC_Email = S.GC_Email,
			T.GC_AddressMap = S.GC_AddressMap,
			T.GC_ValidationStatus = S.GC_ValidationStatus,
			T.GC_SystemCreateTimeUtc = S.GC_SystemCreateTimeUtc,
			T.GC_SystemCreateUser = S.GC_SystemCreateUser,
			T.GC_SystemLastEditTimeUtc = S.GC_SystemLastEditTimeUtc,
			T.GC_SystemLastEditUser = S.GC_SystemLastEditUser
		WHEN NOT MATCHED BY TARGET AND S.OpCode IN (2,3,4) THEN INSERT (
			GC_PK,
			GC_Code,
			GC_Name,
			GC_BusinessRegNo,
			GC_BusinessRegNo2,
			GC_CustomsRegistrationNo,
			GC_Address1,
			GC_Address2,
			GC_City,
			GC_Phone,
			GC_PostCode,
			GC_State,
			GC_Fax,
			GC_WebAddress,
			GC_NoOfAccountingPeriods,
			GC_StartDate,
			GC_OH_OrgProxy,
			GC_RX_NKLocalCurrency,
			GC_RN_NKCountryCode,
			GC_LocalDocLanguage,
			GC_IsActive,
			GC_IsReciprocal,
			GC_IsGSTRegistered,
			GC_IsGSTCashBasis,
			GC_IsWHTRegistered,
			GC_IsWHTCashBasis,
			GC_IsValid,
			GC_Email,
			GC_AddressMap,
			GC_ValidationStatus,
			GC_SystemCreateTimeUtc,
			GC_SystemCreateUser,
			GC_SystemLastEditTimeUtc,
			GC_SystemLastEditUser
		) VALUES (
			S.GC_PK,
			S.GC_Code,
			S.GC_Name,
			S.GC_BusinessRegNo,
			S.GC_BusinessRegNo2,
			S.GC_CustomsRegistrationNo,
			S.GC_Address1,
			S.GC_Address2,
			S.GC_City,
			S.GC_Phone,
			S.GC_PostCode,
			S.GC_State,
			S.GC_Fax,
			S.GC_WebAddress,
			S.GC_NoOfAccountingPeriods,
			S.GC_StartDate,
			S.GC_OH_OrgProxy,
			S.GC_RX_NKLocalCurrency,
			S.GC_RN_NKCountryCode,
			S.GC_LocalDocLanguage,
			S.GC_IsActive,
			S.GC_IsReciprocal,
			S.GC_IsGSTRegistered,
			S.GC_IsGSTCashBasis,
			S.GC_IsWHTRegistered,
			S.GC_IsWHTCashBasis,
			S.GC_IsValid,
			S.GC_Email,
			S.GC_AddressMap,
			S.GC_ValidationStatus,
			S.GC_SystemCreateTimeUtc,
			S.GC_SystemCreateUser,
			S.GC_SystemLastEditTimeUtc,
			S.GC_SystemLastEditUser
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

DROP PROCEDURE IF EXISTS [CWO].[MergeGenCustomAddOnValue]
GO
IF type_id('[CWO].[GenCustomAddOnValue_TVP]') IS NOT NULL
	DROP TYPE [CWO].[GenCustomAddOnValue_TVP];
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'GenCustomAddOnValue' AND xtype='U')
	CREATE TABLE [CWO].[GenCustomAddOnValue] (
		[XV_PK] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_GenCustomAddOnValue] PRIMARY KEY (XV_PK),
		[XV_Type] NVARCHAR(MAX) NULL,
		[XV_Data] NVARCHAR(MAX) NULL,
		[XV_IsRuleEnabled] BIT NULL,
		[XV_XR_Rule] UNIQUEIDENTIFIER NULL,
		[XV_ParentTableCode] NVARCHAR(MAX) NULL,
		[XV_ParentID] UNIQUEIDENTIFIER NULL,
		[XV_AutoVersion] INT NULL,
		[XV_SystemCreateTimeUtc] DATETIME NULL,
		[XV_SystemCreateUser] NVARCHAR(MAX) NULL,
		[XV_SystemLastEditTimeUtc] DATETIME NULL,
		[XV_SystemLastEditUser] NVARCHAR(MAX) NULL,
		[XV_Name] NVARCHAR(MAX) NULL,
		[XV_DataAsDecimal] DECIMAL(19,4) NULL,
		[XV_UploadType] NVARCHAR(3) NOT NULL
	)
GO

CREATE TYPE [CWO].[GenCustomAddOnValue_TVP] AS TABLE (
	[XV_PK] UNIQUEIDENTIFIER NOT NULL,
	[XV_Type] NVARCHAR(MAX) NULL,
	[XV_Data] NVARCHAR(MAX) NULL,
	[XV_IsRuleEnabled] BIT NULL,
	[XV_XR_Rule] UNIQUEIDENTIFIER NULL,
	[XV_ParentTableCode] NVARCHAR(MAX) NULL,
	[XV_ParentID] UNIQUEIDENTIFIER NULL,
	[XV_AutoVersion] INT NULL,
	[XV_SystemCreateTimeUtc] DATETIME NULL,
	[XV_SystemCreateUser] NVARCHAR(MAX) NULL,
	[XV_SystemLastEditTimeUtc] DATETIME NULL,
	[XV_SystemLastEditUser] NVARCHAR(MAX) NULL,
	[XV_Name] NVARCHAR(MAX) NULL,
	[XV_DataAsDecimal] DECIMAL(19,4) NULL,
	[OpCode] INT NOT NULL
)
GO

CREATE PROCEDURE [CWO].[MergeGenCustomAddOnValue]
	 @Items [CWO].[GenCustomAddOnValue_TVP] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRANSACTION;
		MERGE INTO [CWO].[GenCustomAddOnValue] AS T
		USING @Items AS S ON T.XV_PK = S.XV_PK
		WHEN MATCHED AND S.OpCode = 1 THEN DELETE
		WHEN MATCHED AND S.OpCode IN (3,4) THEN UPDATE SET
			T.XV_PK = S.XV_PK,
			T.XV_Type = S.XV_Type,
			T.XV_Data = S.XV_Data,
			T.XV_IsRuleEnabled = S.XV_IsRuleEnabled,
			T.XV_XR_Rule = S.XV_XR_Rule,
			T.XV_ParentTableCode = S.XV_ParentTableCode,
			T.XV_ParentID = S.XV_ParentID,
			T.XV_AutoVersion = S.XV_AutoVersion,
			T.XV_SystemCreateTimeUtc = S.XV_SystemCreateTimeUtc,
			T.XV_SystemCreateUser = S.XV_SystemCreateUser,
			T.XV_SystemLastEditTimeUtc = S.XV_SystemLastEditTimeUtc,
			T.XV_SystemLastEditUser = S.XV_SystemLastEditUser,
			T.XV_Name = S.XV_Name,
			T.XV_DataAsDecimal = S.XV_DataAsDecimal
		WHEN NOT MATCHED BY TARGET AND S.OpCode IN (2,3,4) THEN INSERT (
			XV_PK,
			XV_Type,
			XV_Data,
			XV_IsRuleEnabled,
			XV_XR_Rule,
			XV_ParentTableCode,
			XV_ParentID,
			XV_AutoVersion,
			XV_SystemCreateTimeUtc,
			XV_SystemCreateUser,
			XV_SystemLastEditTimeUtc,
			XV_SystemLastEditUser,
			XV_Name,
			XV_DataAsDecimal,
			XV_UploadType
		) VALUES (
			S.XV_PK,
			S.XV_Type,
			S.XV_Data,
			S.XV_IsRuleEnabled,
			S.XV_XR_Rule,
			S.XV_ParentTableCode,
			S.XV_ParentID,
			S.XV_AutoVersion,
			S.XV_SystemCreateTimeUtc,
			S.XV_SystemCreateUser,
			S.XV_SystemLastEditTimeUtc,
			S.XV_SystemLastEditUser,
			S.XV_Name,
			S.XV_DataAsDecimal,
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

