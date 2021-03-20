
/* Start SY_PermissionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGetByID]
	@Id smallint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[FunctionId],
		[ParentId]
	FROM [SY_Permission]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionGetByID */

/* Start SY_PermissionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[FunctionId],
		[ParentId]
	FROM [SY_Permission]
END
GO
/* End SY_PermissionGetAll */

/* Start SY_PermissionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[FunctionId],
		[ParentId]
	FROM [SY_Permission]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionGetAllOnPage */

/* Start SY_PermissionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionInsert]
	@Name nvarchar(500) = null,
	@Code nvarchar(50) = null,
	@FunctionId smallint = null,
	@ParentId smallint = null
AS
BEGIN
	INSERT INTO [SY_Permission]
	(
		[Name],
		[Code],
		[FunctionId],
		[ParentId]
	)
	VALUES
	(
		@Name,
		@Code,
		@FunctionId,
		@ParentId
	)
END
GO
/* End SY_PermissionGetByID */

/* Start SY_PermissionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUpdate]
	@Id smallint = null,
	@Name nvarchar(500) = null,
	@Code nvarchar(50) = null,
	@FunctionId smallint = null,
	@ParentId smallint = null
AS
BEGIN
	UPDATE [SY_Permission] SET
		[Name] = @Name,
		[Code] = @Code,
		[FunctionId] = @FunctionId,
		[ParentId] = @ParentId
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionUpdate */

/* Start SY_PermissionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionDelete]
	@Id smallint = null
AS
BEGIN
	DELETE [SY_Permission]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionDelete */

/* Start SY_PermissionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionDeleteAll]
AS
BEGIN
	DELETE [SY_Permission]
END
GO
/* End SY_PermissionDeleteAll */
