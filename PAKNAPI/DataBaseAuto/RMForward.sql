
/* Start RM_ForwardGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardGetByID];
GO
CREATE PROCEDURE [dbo].[RM_ForwardGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RemindId],
		[SenderId],
		[SenderName],
		[SendOrgId],
		[ReceiveOrgId],
		[DateSend],
		[IsView]
	FROM [RM_Forward]
	WHERE [Id] = @Id
END
GO
/* End RM_ForwardGetByID */

/* Start RM_ForwardGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardGetAll];
GO
CREATE PROCEDURE [dbo].[RM_ForwardGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RemindId],
		[SenderId],
		[SenderName],
		[SendOrgId],
		[ReceiveOrgId],
		[DateSend],
		[IsView]
	FROM [RM_Forward]
END
GO
/* End RM_ForwardGetAll */

/* Start RM_ForwardGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[RM_ForwardGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RemindId],
		[SenderId],
		[SenderName],
		[SendOrgId],
		[ReceiveOrgId],
		[DateSend],
		[IsView]
	FROM [RM_Forward]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End RM_ForwardGetAllOnPage */

/* Start RM_ForwardInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardInsert];
GO
CREATE PROCEDURE [dbo].[RM_ForwardInsert]
	@RemindId int = null,
	@SenderId int = null,
	@SenderName nvarchar(100) = null,
	@SendOrgId int = null,
	@ReceiveOrgId int = null,
	@DateSend datetime = null,
	@IsView bigint = null
AS
BEGIN
	INSERT INTO [RM_Forward]
	(
		[RemindId],
		[SenderId],
		[SenderName],
		[SendOrgId],
		[ReceiveOrgId],
		[DateSend],
		[IsView]
	)
	VALUES
	(
		@RemindId,
		@SenderId,
		@SenderName,
		@SendOrgId,
		@ReceiveOrgId,
		@DateSend,
		@IsView
	)
END
GO
/* End RM_ForwardGetByID */

/* Start RM_ForwardUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardUpdate];
GO
CREATE PROCEDURE [dbo].[RM_ForwardUpdate]
	@Id int = null,
	@RemindId int = null,
	@SenderId int = null,
	@SenderName nvarchar(100) = null,
	@SendOrgId int = null,
	@ReceiveOrgId int = null,
	@DateSend datetime = null,
	@IsView bigint = null
AS
BEGIN
	UPDATE [RM_Forward] SET
		[RemindId] = @RemindId,
		[SenderId] = @SenderId,
		[SenderName] = @SenderName,
		[SendOrgId] = @SendOrgId,
		[ReceiveOrgId] = @ReceiveOrgId,
		[DateSend] = @DateSend,
		[IsView] = @IsView
	WHERE [Id] = @Id
END
GO
/* End RM_ForwardUpdate */

/* Start RM_ForwardDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardDelete];
GO
CREATE PROCEDURE [dbo].[RM_ForwardDelete]
	@Id int = null
AS
BEGIN
	DELETE [RM_Forward]
	WHERE [Id] = @Id
END
GO
/* End RM_ForwardDelete */

/* Start RM_ForwardDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_ForwardDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_ForwardDeleteAll];
GO
CREATE PROCEDURE [dbo].[RM_ForwardDeleteAll]
AS
BEGIN
	DELETE [RM_Forward]
END
GO
/* End RM_ForwardDeleteAll */
