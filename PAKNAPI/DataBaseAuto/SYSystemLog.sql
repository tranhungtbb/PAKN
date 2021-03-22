
/* Start SY_SystemLogGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SystemLogGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogGetByID];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[Status],
		[Action],
		[Exception],
		[FullName],
		[IPAddress],
		[MACAddress],
		[Description],
		[CreatedDate]
	FROM [SY_SystemLog]
	WHERE [Id] = @Id
END
GO
/* End SY_SystemLogGetByID */

/* Start SY_SystemLogGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_SystemLogGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogGetAll];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogGetAll]
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[Status],
		[Action],
		[Exception],
		[FullName],
		[IPAddress],
		[MACAddress],
		[Description],
		[CreatedDate]
	FROM [SY_SystemLog]
END
GO
/* End SY_SystemLogGetAll */

/* Start SY_SystemLogGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYSystemLogGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[UserId],
		[Status],
		[Action],
		[Exception],
		[FullName],
		[IPAddress],
		[MACAddress],
		[Description],
		[CreatedDate]
	FROM [SY_SystemLog]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_SystemLogGetAllOnPage */

/* Start SY_SystemLogInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYSystemLogInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogInsert];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogInsert]
	@UserId bigint = null,
	@Status tinyint = null,
	@Action nvarchar(256) = null,
	@Exception nvarchar(1000) = null,
	@FullName nvarchar(256) = null,
	@IPAddress nvarchar(256) = null,
	@MACAddress nvarchar(256) = null,
	@Description nvarchar(1000) = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [SY_SystemLog]
	(
		[UserId],
		[Status],
		[Action],
		[Exception],
		[FullName],
		[IPAddress],
		[MACAddress],
		[Description],
		[CreatedDate]
	)
	VALUES
	(
		@UserId,
		@Status,
		@Action,
		@Exception,
		@FullName,
		@IPAddress,
		@MACAddress,
		@Description,
		@CreatedDate
	)
END
GO
/* End SY_SystemLogGetByID */

/* Start SY_SystemLogUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYSystemLogUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogUpdate];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogUpdate]
	@Id bigint = null,
	@UserId bigint = null,
	@Status tinyint = null,
	@Action nvarchar(256) = null,
	@Exception nvarchar(1000) = null,
	@FullName nvarchar(256) = null,
	@IPAddress nvarchar(256) = null,
	@MACAddress nvarchar(256) = null,
	@Description nvarchar(1000) = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [SY_SystemLog] SET
		[UserId] = @UserId,
		[Status] = @Status,
		[Action] = @Action,
		[Exception] = @Exception,
		[FullName] = @FullName,
		[IPAddress] = @IPAddress,
		[MACAddress] = @MACAddress,
		[Description] = @Description,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End SY_SystemLogUpdate */

/* Start SY_SystemLogDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYSystemLogDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogDelete];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_SystemLog]
	WHERE [Id] = @Id
END
GO
/* End SY_SystemLogDelete */

/* Start SY_SystemLogDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYSystemLogDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_SystemLogDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_SystemLogDeleteAll]
AS
BEGIN
	DELETE [SY_SystemLog]
END
GO
/* End SY_SystemLogDeleteAll */
