
/* Start SY_EmailGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailGetByID];
GO
CREATE PROCEDURE [dbo].[SY_EmailGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Email],
		[Password],
		[Server],
		[Port]
	FROM [SY_Email]
	WHERE [Id] = @Id
END
GO
/* End SY_EmailGetByID */

/* Start SY_EmailGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailGetAll];
GO
CREATE PROCEDURE [dbo].[SY_EmailGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Email],
		[Password],
		[Server],
		[Port]
	FROM [SY_Email]
END
GO
/* End SY_EmailGetAll */

/* Start SY_EmailGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_EmailGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Email],
		[Password],
		[Server],
		[Port]
	FROM [SY_Email]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_EmailGetAllOnPage */

/* Start SY_EmailInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailInsert];
GO
CREATE PROCEDURE [dbo].[SY_EmailInsert]
	@Email nvarchar(255) = null,
	@Password nvarchar(255) = null,
	@Server nvarchar(255) = null,
	@Port nvarchar(255) = null
AS
BEGIN
	INSERT INTO [SY_Email]
	(
		[Email],
		[Password],
		[Server],
		[Port]
	)
	VALUES
	(
		@Email,
		@Password,
		@Server,
		@Port
	)
END
GO
/* End SY_EmailGetByID */

/* Start SY_EmailUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailUpdate];
GO
CREATE PROCEDURE [dbo].[SY_EmailUpdate]
	@Id int = null,
	@Email nvarchar(255) = null,
	@Password nvarchar(255) = null,
	@Server nvarchar(255) = null,
	@Port nvarchar(255) = null
AS
BEGIN
	UPDATE [SY_Email] SET
		[Email] = @Email,
		[Password] = @Password,
		[Server] = @Server,
		[Port] = @Port
	WHERE [Id] = @Id
END
GO
/* End SY_EmailUpdate */

/* Start SY_EmailDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailDelete];
GO
CREATE PROCEDURE [dbo].[SY_EmailDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_Email]
	WHERE [Id] = @Id
END
GO
/* End SY_EmailDelete */

/* Start SY_EmailDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_EmailDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_EmailDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_EmailDeleteAll]
AS
BEGIN
	DELETE [SY_Email]
END
GO
/* End SY_EmailDeleteAll */
