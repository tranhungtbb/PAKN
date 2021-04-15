
/* Start SYFunctionActionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionGetByID];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[FunctionId]
	FROM [SYFunctionAction]
	WHERE [Id] = @Id
END
GO
/* End SYFunctionActionGetByID */

/* Start SYFunctionActionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionGetAll];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[FunctionId]
	FROM [SYFunctionAction]
END
GO
/* End SYFunctionActionGetAll */

/* Start SYFunctionActionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[FunctionId]
	FROM [SYFunctionAction]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYFunctionActionGetAllOnPage */

/* Start SYFunctionActionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionInsert];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionInsert]
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@FunctionId bigint = null
AS
BEGIN
	INSERT INTO [SYFunctionAction]
	(
		[Name],
		[Code],
		[FunctionId]
	)
	VALUES
	(
		@Name,
		@Code,
		@FunctionId
	)
END
GO
/* End SYFunctionActionGetByID */

/* Start SYFunctionActionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionUpdate];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionUpdate]
	@Id bigint = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@FunctionId bigint = null
AS
BEGIN
	UPDATE [SYFunctionAction] SET
		[Name] = @Name,
		[Code] = @Code,
		[FunctionId] = @FunctionId
	WHERE [Id] = @Id
END
GO
/* End SYFunctionActionUpdate */

/* Start SYFunctionActionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionDelete];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYFunctionAction]
	WHERE [Id] = @Id
END
GO
/* End SYFunctionActionDelete */

/* Start SYFunctionActionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionActionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionActionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYFunctionActionDeleteAll]
AS
BEGIN
	DELETE [SYFunctionAction]
END
GO
/* End SYFunctionActionDeleteAll */
