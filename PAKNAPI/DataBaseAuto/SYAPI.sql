
/* Start SY_APIGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIGetByID];
GO
CREATE PROCEDURE [dbo].[SY_APIGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Authorize]
	FROM [SY_API]
	WHERE [Id] = @Id
END
GO
/* End SY_APIGetByID */

/* Start SY_APIGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIGetAll];
GO
CREATE PROCEDURE [dbo].[SY_APIGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Authorize]
	FROM [SY_API]
END
GO
/* End SY_APIGetAll */

/* Start SY_APIGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_APIGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Authorize]
	FROM [SY_API]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_APIGetAllOnPage */

/* Start SY_APIInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIInsert];
GO
CREATE PROCEDURE [dbo].[SY_APIInsert]
	@Name nvarchar(150) = null,
	@Authorize bit = null
AS
BEGIN
	INSERT INTO [SY_API]
	(
		[Name],
		[Authorize]
	)
	VALUES
	(
		@Name,
		@Authorize
	)
END
GO
/* End SY_APIGetByID */

/* Start SY_APIUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIUpdate];
GO
CREATE PROCEDURE [dbo].[SY_APIUpdate]
	@Id int = null,
	@Name nvarchar(150) = null,
	@Authorize bit = null
AS
BEGIN
	UPDATE [SY_API] SET
		[Name] = @Name,
		[Authorize] = @Authorize
	WHERE [Id] = @Id
END
GO
/* End SY_APIUpdate */

/* Start SY_APIDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIDelete];
GO
CREATE PROCEDURE [dbo].[SY_APIDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_API]
	WHERE [Id] = @Id
END
GO
/* End SY_APIDelete */

/* Start SY_APIDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APIDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APIDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_APIDeleteAll]
AS
BEGIN
	DELETE [SY_API]
END
GO
/* End SY_APIDeleteAll */

/* Start SY_APICount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_APICount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_APICount];
GO
CREATE PROCEDURE [dbo].[SY_APICount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_API]
END
GO
/* End SY_APICount */
