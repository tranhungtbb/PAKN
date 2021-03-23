
/* Start SYAPIGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIGetByID];
GO
CREATE PROCEDURE [dbo].[SYAPIGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[AllowAnonymous]
	FROM [SYAPI]
	WHERE [Id] = @Id
END
GO
/* End SYAPIGetByID */

/* Start SYAPIGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIGetAll];
GO
CREATE PROCEDURE [dbo].[SYAPIGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[AllowAnonymous]
	FROM [SYAPI]
END
GO
/* End SYAPIGetAll */

/* Start SYAPIGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYAPIGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[AllowAnonymous]
	FROM [SYAPI]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYAPIGetAllOnPage */

/* Start SYAPIInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIInsert];
GO
CREATE PROCEDURE [dbo].[SYAPIInsert]
	@Name nvarchar(256) = null,
	@AllowAnonymous bit = null
AS
BEGIN
	INSERT INTO [SYAPI]
	(
		[Name],
		[AllowAnonymous]
	)
	VALUES
	(
		@Name,
		@AllowAnonymous
	)
END
GO
/* End SYAPIGetByID */

/* Start SYAPIUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIUpdate];
GO
CREATE PROCEDURE [dbo].[SYAPIUpdate]
	@Id bigint = null,
	@Name nvarchar(256) = null,
	@AllowAnonymous bit = null
AS
BEGIN
	UPDATE [SYAPI] SET
		[Name] = @Name,
		[AllowAnonymous] = @AllowAnonymous
	WHERE [Id] = @Id
END
GO
/* End SYAPIUpdate */

/* Start SYAPIDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIDelete];
GO
CREATE PROCEDURE [dbo].[SYAPIDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYAPI]
	WHERE [Id] = @Id
END
GO
/* End SYAPIDelete */

/* Start SYAPIDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYAPIDeleteAll]
AS
BEGIN
	DELETE [SYAPI]
END
GO
/* End SYAPIDeleteAll */
