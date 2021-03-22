
/* Start SY_PermissionCategoryGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryGetByID]
	@Id smallint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code]
	FROM [SY_PermissionCategory]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionCategoryGetByID */

/* Start SY_PermissionCategoryGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code]
	FROM [SY_PermissionCategory]
END
GO
/* End SY_PermissionCategoryGetAll */

/* Start SY_PermissionCategoryGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code]
	FROM [SY_PermissionCategory]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionCategoryGetAllOnPage */

/* Start SY_PermissionCategoryInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryInsert]
	@Name nvarchar(500) = null,
	@Code nvarchar(25) = null
AS
BEGIN
	INSERT INTO [SY_PermissionCategory]
	(
		[Name],
		[Code]
	)
	VALUES
	(
		@Name,
		@Code
	)
END
GO
/* End SY_PermissionCategoryGetByID */

/* Start SY_PermissionCategoryUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryUpdate]
	@Id smallint = null,
	@Name nvarchar(500) = null,
	@Code nvarchar(25) = null
AS
BEGIN
	UPDATE [SY_PermissionCategory] SET
		[Name] = @Name,
		[Code] = @Code
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionCategoryUpdate */

/* Start SY_PermissionCategoryDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryDelete]
	@Id smallint = null
AS
BEGIN
	DELETE [SY_PermissionCategory]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionCategoryDelete */

/* Start SY_PermissionCategoryDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionCategoryDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionCategoryDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionCategoryDeleteAll]
AS
BEGIN
	DELETE [SY_PermissionCategory]
END
GO
/* End SY_PermissionCategoryDeleteAll */
