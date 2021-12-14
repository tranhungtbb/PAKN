
/* Start SYPositionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionGetByID];
GO
CREATE PROCEDURE [dbo].[SYPositionGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[OrganizationId],
		[IsActive]
	FROM [SYPosition]
	WHERE [Id] = @Id
END
GO
/* End SYPositionGetByID */

/* Start SYPositionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionGetAll];
GO
CREATE PROCEDURE [dbo].[SYPositionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[OrganizationId],
		[IsActive]
	FROM [SYPosition]
END
GO
/* End SYPositionGetAll */

/* Start SYPositionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYPositionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[OrganizationId],
		[IsActive]
	FROM [SYPosition]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYPositionGetAllOnPage */

/* Start SYPositionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionInsert];
GO
CREATE PROCEDURE [dbo].[SYPositionInsert]
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@OrganizationId bigint = null,
	@IsActive bit = null
AS
BEGIN
	INSERT INTO [SYPosition]
	(
		[Name],
		[Code],
		[OrganizationId],
		[IsActive]
	)
	VALUES
	(
		@Name,
		@Code,
		@OrganizationId,
		@IsActive
	)
END
GO
/* End SYPositionGetByID */

/* Start SYPositionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionUpdate];
GO
CREATE PROCEDURE [dbo].[SYPositionUpdate]
	@Id bigint = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@OrganizationId bigint = null,
	@IsActive bit = null
AS
BEGIN
	UPDATE [SYPosition] SET
		[Name] = @Name,
		[Code] = @Code,
		[OrganizationId] = @OrganizationId,
		[IsActive] = @IsActive
	WHERE [Id] = @Id
END
GO
/* End SYPositionUpdate */

/* Start SYPositionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionDelete];
GO
CREATE PROCEDURE [dbo].[SYPositionDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYPosition]
	WHERE [Id] = @Id
END
GO
/* End SYPositionDelete */

/* Start SYPositionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYPositionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYPositionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYPositionDeleteAll]
AS
BEGIN
	DELETE [SYPosition]
END
GO
/* End SYPositionDeleteAll */
