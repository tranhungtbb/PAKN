
/* Start SY_NotificationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationGetByID];
GO
CREATE PROCEDURE [dbo].[SY_NotificationGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[ReceiveId],
		[SenderId],
		[SendOrgId],
		[DataId],
		[SendDate],
		[Type],
		[Title],
		[Content],
		[IsViewed],
		[TypeSend],
		[ReceiveOrgId]
	FROM [SY_Notification]
	WHERE [Id] = @Id
END
GO
/* End SY_NotificationGetByID */

/* Start SY_NotificationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationGetAll];
GO
CREATE PROCEDURE [dbo].[SY_NotificationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[ReceiveId],
		[SenderId],
		[SendOrgId],
		[DataId],
		[SendDate],
		[Type],
		[Title],
		[Content],
		[IsViewed],
		[TypeSend],
		[ReceiveOrgId]
	FROM [SY_Notification]
END
GO
/* End SY_NotificationGetAll */

/* Start SY_NotificationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_NotificationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[ReceiveId],
		[SenderId],
		[SendOrgId],
		[DataId],
		[SendDate],
		[Type],
		[Title],
		[Content],
		[IsViewed],
		[TypeSend],
		[ReceiveOrgId]
	FROM [SY_Notification]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_NotificationGetAllOnPage */

/* Start SY_NotificationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationInsert];
GO
CREATE PROCEDURE [dbo].[SY_NotificationInsert]
	@ReceiveId int = null,
	@SenderId int = null,
	@SendOrgId int = null,
	@DataId int = null,
	@SendDate datetime = null,
	@Type smallint = null,
	@Title nvarchar(150) = null,
	@Content nvarchar(1000) = null,
	@IsViewed bit = null,
	@TypeSend smallint = null,
	@ReceiveOrgId int = null
AS
BEGIN
	INSERT INTO [SY_Notification]
	(
		[ReceiveId],
		[SenderId],
		[SendOrgId],
		[DataId],
		[SendDate],
		[Type],
		[Title],
		[Content],
		[IsViewed],
		[TypeSend],
		[ReceiveOrgId]
	)
	VALUES
	(
		@ReceiveId,
		@SenderId,
		@SendOrgId,
		@DataId,
		@SendDate,
		@Type,
		@Title,
		@Content,
		@IsViewed,
		@TypeSend,
		@ReceiveOrgId
	)
END
GO
/* End SY_NotificationGetByID */

/* Start SY_NotificationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationUpdate];
GO
CREATE PROCEDURE [dbo].[SY_NotificationUpdate]
	@Id bigint = null,
	@ReceiveId int = null,
	@SenderId int = null,
	@SendOrgId int = null,
	@DataId int = null,
	@SendDate datetime = null,
	@Type smallint = null,
	@Title nvarchar(150) = null,
	@Content nvarchar(1000) = null,
	@IsViewed bit = null,
	@TypeSend smallint = null,
	@ReceiveOrgId int = null
AS
BEGIN
	UPDATE [SY_Notification] SET
		[ReceiveId] = @ReceiveId,
		[SenderId] = @SenderId,
		[SendOrgId] = @SendOrgId,
		[DataId] = @DataId,
		[SendDate] = @SendDate,
		[Type] = @Type,
		[Title] = @Title,
		[Content] = @Content,
		[IsViewed] = @IsViewed,
		[TypeSend] = @TypeSend,
		[ReceiveOrgId] = @ReceiveOrgId
	WHERE [Id] = @Id
END
GO
/* End SY_NotificationUpdate */

/* Start SY_NotificationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationDelete];
GO
CREATE PROCEDURE [dbo].[SY_NotificationDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_Notification]
	WHERE [Id] = @Id
END
GO
/* End SY_NotificationDelete */

/* Start SY_NotificationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_NotificationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_NotificationDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_NotificationDeleteAll]
AS
BEGIN
	DELETE [SY_Notification]
END
GO
/* End SY_NotificationDeleteAll */
