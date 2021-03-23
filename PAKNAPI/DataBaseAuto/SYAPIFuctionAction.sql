
/* Start SYAPIFuctionActionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionGetByID];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[APIId],
		[FuctionActionId]
	FROM [SYAPIFuctionAction]
	WHERE [Id] = @Id
END
GO
/* End SYAPIFuctionActionGetByID */

/* Start SYAPIFuctionActionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionGetAll];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[APIId],
		[FuctionActionId]
	FROM [SYAPIFuctionAction]
END
GO
/* End SYAPIFuctionActionGetAll */

/* Start SYAPIFuctionActionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[APIId],
		[FuctionActionId]
	FROM [SYAPIFuctionAction]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SYAPIFuctionActionGetAllOnPage */

/* Start SYAPIFuctionActionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionInsert];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionInsert]
	@APIId bigint = null,
	@FuctionActionId bigint = null
AS
BEGIN
	INSERT INTO [SYAPIFuctionAction]
	(
		[APIId],
		[FuctionActionId]
	)
	VALUES
	(
		@APIId,
		@FuctionActionId
	)
END
GO
/* End SYAPIFuctionActionGetByID */

/* Start SYAPIFuctionActionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionUpdate];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionUpdate]
	@Id bigint = null,
	@APIId bigint = null,
	@FuctionActionId bigint = null
AS
BEGIN
	UPDATE [SYAPIFuctionAction] SET
		[APIId] = @APIId,
		[FuctionActionId] = @FuctionActionId
	WHERE [Id] = @Id
END
GO
/* End SYAPIFuctionActionUpdate */

/* Start SYAPIFuctionActionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionDelete];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SYAPIFuctionAction]
	WHERE [Id] = @Id
END
GO
/* End SYAPIFuctionActionDelete */

/* Start SYAPIFuctionActionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYAPIFuctionActionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SYAPIFuctionActionDeleteAll];
GO
CREATE PROCEDURE [dbo].[SYAPIFuctionActionDeleteAll]
AS
BEGIN
	DELETE [SYAPIFuctionAction]
END
GO
/* End SYAPIFuctionActionDeleteAll */
