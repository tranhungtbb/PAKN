
/* Start SYUserGroupGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupGetByID];
GO
CREATE PROCEDURE [dbo].[SYUserGroupGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[IsActive],
		[Name],
		[Code]
	FROM [SYUserGroup]
	WHERE [Id] = @Id
END
GO
/* End SYUserGroupGetByID */

/* Start SYUserGroupGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupGetAll];
GO
CREATE PROCEDURE [dbo].[SYUserGroupGetAll]
AS
BEGIN
	SELECT
		[Id],
		[IsActive],
		[Name],
		[Code]
	FROM [SYUserGroup]
END
GO
/* End SYUserGroupGetAll */

/* Start SYUserGroupGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYUserGroupGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[IsActive],
		[Name],
		[Code]
	FROM [SYUserGroup]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYUserGroupGetAllOnPage */

/* Start SYUserGroupInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupInsert];
GO
CREATE PROCEDURE [dbo].[SYUserGroupInsert]
	@IsActive bit = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null
AS
BEGIN
	INSERT INTO [SYUserGroup]
	(
		[IsActive],
		[Name],
		[Code]
	)
	VALUES
	(
		@IsActive,
		@Name,
		@Code
	)
END
GO
/* End SYUserGroupGetByID */

/* Start SYUserGroupUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupUpdate];
GO
CREATE PROCEDURE [dbo].[SYUserGroupUpdate]
	@Id bigint = null,
	@IsActive bit = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null
AS
BEGIN
	UPDATE [SYUserGroup] SET
		[IsActive] = @IsActive,
		[Name] = @Name,
		[Code] = @Code
	WHERE [Id] = @Id
END
GO
/* End SYUserGroupUpdate */

/* Start SYUserGroupDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupDelete];
GO
CREATE PROCEDURE [dbo].[SYUserGroupDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYUserGroup]
	WHERE [Id] = @Id
END
GO
/* End SYUserGroupDelete */

/* Start SYUserGroupDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYUserGroupDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYUserGroupDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYUserGroupDeleteAll]
AS
BEGIN
	DELETE [SYUserGroup]
END
GO
/* End SYUserGroupDeleteAll */
