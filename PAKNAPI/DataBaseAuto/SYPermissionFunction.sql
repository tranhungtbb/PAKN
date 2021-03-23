
/* Start SY_PermissionFunctionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionGetByID]
	@Id smallint = null
AS
BEGIN
	SELECT
		[Id],
		[CategoryId],
		[Name],
		[Code]
	FROM [SY_PermissionFunction]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionFunctionGetByID */

/* Start SY_PermissionFunctionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[CategoryId],
		[Name],
		[Code]
	FROM [SY_PermissionFunction]
END
GO
/* End SY_PermissionFunctionGetAll */

/* Start SY_PermissionFunctionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[CategoryId],
		[Name],
		[Code]
	FROM [SY_PermissionFunction]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionFunctionGetAllOnPage */

/* Start SY_PermissionFunctionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionInsert]
	@CategoryId smallint = null,
	@Name nvarchar(500) = null,
	@Code nvarchar(50) = null
AS
BEGIN
	INSERT INTO [SY_PermissionFunction]
	(
		[CategoryId],
		[Name],
		[Code]
	)
	VALUES
	(
		@CategoryId,
		@Name,
		@Code
	)
END
GO
/* End SY_PermissionFunctionGetByID */

/* Start SY_PermissionFunctionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionUpdate]
	@Id smallint = null,
	@CategoryId smallint = null,
	@Name nvarchar(500) = null,
	@Code nvarchar(50) = null
AS
BEGIN
	UPDATE [SY_PermissionFunction] SET
		[CategoryId] = @CategoryId,
		[Name] = @Name,
		[Code] = @Code
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionFunctionUpdate */

/* Start SY_PermissionFunctionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionDelete]
	@Id smallint = null
AS
BEGIN
	DELETE [SY_PermissionFunction]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionFunctionDelete */

/* Start SY_PermissionFunctionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionDeleteAll]
AS
BEGIN
	DELETE [SY_PermissionFunction]
END
GO
/* End SY_PermissionFunctionDeleteAll */

/* Start SY_PermissionFunctionCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionFunctionCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionFunctionCount];
GO
CREATE PROCEDURE [dbo].[SY_PermissionFunctionCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_PermissionFunction]
END
GO
/* End SY_PermissionFunctionCount */
