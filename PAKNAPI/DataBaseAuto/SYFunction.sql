
/* Start SYFunctionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionGetByID];
GO
CREATE PROCEDURE [dbo].[SYFunctionGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code]
	FROM [SYFunction]
	WHERE [Id] = @Id
END
GO
/* End SYFunctionGetByID */

/* Start SYFunctionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionGetAll];
GO
CREATE PROCEDURE [dbo].[SYFunctionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code]
	FROM [SYFunction]
END
GO
/* End SYFunctionGetAll */

/* Start SYFunctionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYFunctionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code]
	FROM [SYFunction]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYFunctionGetAllOnPage */

/* Start SYFunctionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionInsert];
GO
CREATE PROCEDURE [dbo].[SYFunctionInsert]
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null
AS
BEGIN
	INSERT INTO [SYFunction]
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
/* End SYFunctionGetByID */

/* Start SYFunctionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionUpdate];
GO
CREATE PROCEDURE [dbo].[SYFunctionUpdate]
	@Id bigint = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null
AS
BEGIN
	UPDATE [SYFunction] SET
		[Name] = @Name,
		[Code] = @Code
	WHERE [Id] = @Id
END
GO
/* End SYFunctionUpdate */

/* Start SYFunctionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionDelete];
GO
CREATE PROCEDURE [dbo].[SYFunctionDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYFunction]
	WHERE [Id] = @Id
END
GO
/* End SYFunctionDelete */

/* Start SYFunctionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYFunctionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYFunctionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYFunctionDeleteAll]
AS
BEGIN
	DELETE [SYFunction]
END
GO
/* End SYFunctionDeleteAll */
