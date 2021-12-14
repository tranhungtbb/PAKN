
/* Start SY_SMSGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSGetByID];
GO
CREATE PROCEDURE [dbo].[SY_SMSGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Linkwebservice],
		[User],
		[Password],
		[Code],
		[ServiceID],
		[CommandCode],
		[ContenType]
	FROM [SY_SMS]
	WHERE [Id] = @Id
END
GO
/* End SY_SMSGetByID */

/* Start SY_SMSGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSGetAll];
GO
CREATE PROCEDURE [dbo].[SY_SMSGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Linkwebservice],
		[User],
		[Password],
		[Code],
		[ServiceID],
		[CommandCode],
		[ContenType]
	FROM [SY_SMS]
END
GO
/* End SY_SMSGetAll */

/* Start SY_SMSGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_SMSGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Linkwebservice],
		[User],
		[Password],
		[Code],
		[ServiceID],
		[CommandCode],
		[ContenType]
	FROM [SY_SMS]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_SMSGetAllOnPage */

/* Start SY_SMSInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSInsert];
GO
CREATE PROCEDURE [dbo].[SY_SMSInsert]
	@Linkwebservice nvarchar(255) = null,
	@User nvarchar(255) = null,
	@Password nvarchar(255) = null,
	@Code nvarchar(255) = null,
	@ServiceID nvarchar(255) = null,
	@CommandCode nvarchar(255) = null,
	@ContenType bit = null
AS
BEGIN
	INSERT INTO [SY_SMS]
	(
		[Linkwebservice],
		[User],
		[Password],
		[Code],
		[ServiceID],
		[CommandCode],
		[ContenType]
	)
	VALUES
	(
		@Linkwebservice,
		@User,
		@Password,
		@Code,
		@ServiceID,
		@CommandCode,
		@ContenType
	)
END
GO
/* End SY_SMSGetByID */

/* Start SY_SMSUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSUpdate];
GO
CREATE PROCEDURE [dbo].[SY_SMSUpdate]
	@Id int = null,
	@Linkwebservice nvarchar(255) = null,
	@User nvarchar(255) = null,
	@Password nvarchar(255) = null,
	@Code nvarchar(255) = null,
	@ServiceID nvarchar(255) = null,
	@CommandCode nvarchar(255) = null,
	@ContenType bit = null
AS
BEGIN
	UPDATE [SY_SMS] SET
		[Linkwebservice] = @Linkwebservice,
		[User] = @User,
		[Password] = @Password,
		[Code] = @Code,
		[ServiceID] = @ServiceID,
		[CommandCode] = @CommandCode,
		[ContenType] = @ContenType
	WHERE [Id] = @Id
END
GO
/* End SY_SMSUpdate */

/* Start SY_SMSDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSDelete];
GO
CREATE PROCEDURE [dbo].[SY_SMSDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_SMS]
	WHERE [Id] = @Id
END
GO
/* End SY_SMSDelete */

/* Start SY_SMSDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SMSDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SMSDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_SMSDeleteAll]
AS
BEGIN
	DELETE [SY_SMS]
END
GO
/* End SY_SMSDeleteAll */
