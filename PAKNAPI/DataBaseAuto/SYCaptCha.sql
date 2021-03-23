
/* Start SY_CaptChaGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaGetByID];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[CaptchaCode],
		[CreateTime]
	FROM [SY_CaptCha]
	WHERE [Id] = @Id
END
GO
/* End SY_CaptChaGetByID */

/* Start SY_CaptChaGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaGetAll];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaGetAll]
AS
BEGIN
	SELECT
		[Id],
		[CaptchaCode],
		[CreateTime]
	FROM [SY_CaptCha]
END
GO
/* End SY_CaptChaGetAll */

/* Start SY_CaptChaGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[CaptchaCode],
		[CreateTime]
	FROM [SY_CaptCha]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_CaptChaGetAllOnPage */

/* Start SY_CaptChaInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaInsert];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaInsert]
	@CaptchaCode varchar(5) = null,
	@CreateTime datetime = null
AS
BEGIN
	INSERT INTO [SY_CaptCha]
	(
		[CaptchaCode],
		[CreateTime]
	)
	VALUES
	(
		@CaptchaCode,
		@CreateTime
	)
END
GO
/* End SY_CaptChaGetByID */

/* Start SY_CaptChaUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaUpdate];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaUpdate]
	@Id int = null,
	@CaptchaCode varchar(5) = null,
	@CreateTime datetime = null
AS
BEGIN
	UPDATE [SY_CaptCha] SET
		[CaptchaCode] = @CaptchaCode,
		[CreateTime] = @CreateTime
	WHERE [Id] = @Id
END
GO
/* End SY_CaptChaUpdate */

/* Start SY_CaptChaDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaDelete];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_CaptCha]
	WHERE [Id] = @Id
END
GO
/* End SY_CaptChaDelete */

/* Start SY_CaptChaDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaDeleteAll]
AS
BEGIN
	DELETE [SY_CaptCha]
END
GO
/* End SY_CaptChaDeleteAll */

/* Start SY_CaptChaCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_CaptChaCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_CaptChaCount];
GO
CREATE PROCEDURE [dbo].[SY_CaptChaCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_CaptCha]
END
GO
/* End SY_CaptChaCount */
