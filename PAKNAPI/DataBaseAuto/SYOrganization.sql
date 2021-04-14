
/* Start SYOrganizationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationGetByID];
GO
CREATE PROCEDURE [dbo].[SYOrganizationGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[Email],
		[Phone],
		[Description],
		[IsActive],
		[IsDelete],
		[ManagerId]
	FROM [SYOrganization]
	WHERE [Id] = @Id
END
GO
/* End SYOrganizationGetByID */

/* Start SYOrganizationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationGetAll];
GO
CREATE PROCEDURE [dbo].[SYOrganizationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[Email],
		[Phone],
		[Description],
		[IsActive],
		[IsDelete],
		[ManagerId]
	FROM [SYOrganization]
END
GO
/* End SYOrganizationGetAll */

/* Start SYOrganizationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYOrganizationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[Email],
		[Phone],
		[Description],
		[IsActive],
		[IsDelete],
		[ManagerId]
	FROM [SYOrganization]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYOrganizationGetAllOnPage */

/* Start SYOrganizationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationInsert];
GO
CREATE PROCEDURE [dbo].[SYOrganizationInsert]
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@Email nvarchar(256) = null,
	@Phone nvarchar(256) = null,
	@Description nvarchar(256) = null,
	@IsActive bit = null,
	@IsDelete bit = null,
	@ManagerId bigint = null
AS
BEGIN
	INSERT INTO [SYOrganization]
	(
		[Name],
		[Code],
		[Email],
		[Phone],
		[Description],
		[IsActive],
		[IsDelete],
		[ManagerId]
	)
	VALUES
	(
		@Name,
		@Code,
		@Email,
		@Phone,
		@Description,
		@IsActive,
		@IsDelete,
		@ManagerId
	)
END
GO
/* End SYOrganizationGetByID */

/* Start SYOrganizationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationUpdate];
GO
CREATE PROCEDURE [dbo].[SYOrganizationUpdate]
	@Id bigint = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@Email nvarchar(256) = null,
	@Phone nvarchar(256) = null,
	@Description nvarchar(256) = null,
	@IsActive bit = null,
	@IsDelete bit = null,
	@ManagerId bigint = null
AS
BEGIN
	UPDATE [SYOrganization] SET
		[Name] = @Name,
		[Code] = @Code,
		[Email] = @Email,
		[Phone] = @Phone,
		[Description] = @Description,
		[IsActive] = @IsActive,
		[IsDelete] = @IsDelete,
		[ManagerId] = @ManagerId
	WHERE [Id] = @Id
END
GO
/* End SYOrganizationUpdate */

/* Start SYOrganizationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationDelete];
GO
CREATE PROCEDURE [dbo].[SYOrganizationDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYOrganization]
	WHERE [Id] = @Id
END
GO
/* End SYOrganizationDelete */

/* Start SYOrganizationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYOrganizationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYOrganizationDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYOrganizationDeleteAll]
AS
BEGIN
	DELETE [SYOrganization]
END
GO
/* End SYOrganizationDeleteAll */
