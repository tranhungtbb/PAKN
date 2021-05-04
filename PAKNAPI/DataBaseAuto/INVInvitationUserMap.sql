
/* Start INV_Invitation_User_MapGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapGetByID];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapGetByID]
	@UserId int = null
AS
BEGIN
	SELECT
		[UserId],
		[InvitationId],
		[Watched],
		[SendEmail],
		[SendSMS]
	FROM [INV_Invitation_User_Map]
	WHERE [UserId] = @UserId
END
GO
/* End INV_Invitation_User_MapGetByID */

/* Start INV_Invitation_User_MapGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapGetAll];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapGetAll]
AS
BEGIN
	SELECT
		[UserId],
		[InvitationId],
		[Watched],
		[SendEmail],
		[SendSMS]
	FROM [INV_Invitation_User_Map]
END
GO
/* End INV_Invitation_User_MapGetAll */

/* Start INV_Invitation_User_MapGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[UserId],
		[InvitationId],
		[Watched],
		[SendEmail],
		[SendSMS]
	FROM [INV_Invitation_User_Map]
	ORDER BY [UserId,InvitationId]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End INV_Invitation_User_MapGetAllOnPage */

/* Start INV_Invitation_User_MapInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapInsert];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapInsert]
	@Watched bit = null,
	@SendEmail bit = null,
	@SendSMS bit = null
AS
BEGIN
	INSERT INTO [INV_Invitation_User_Map]
	(
		[Watched],
		[SendEmail],
		[SendSMS]
	)
	VALUES
	(
		@Watched,
		@SendEmail,
		@SendSMS
	)
END
GO
/* End INV_Invitation_User_MapGetByID */

/* Start INV_Invitation_User_MapUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapUpdate];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapUpdate]
	@UserId int = null,
	@InvitationId int = null,
	@Watched bit = null,
	@SendEmail bit = null,
	@SendSMS bit = null
AS
BEGIN
	UPDATE [INV_Invitation_User_Map] SET
		[Watched] = @Watched,
		[SendEmail] = @SendEmail,
		[SendSMS] = @SendSMS
	WHERE [UserId] = @UserId
END
GO
/* End INV_Invitation_User_MapUpdate */

/* Start INV_Invitation_User_MapDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapDelete];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapDelete]
	@UserId int = null
AS
BEGIN
	DELETE [INV_Invitation_User_Map]
	WHERE [UserId] = @UserId
END
GO
/* End INV_Invitation_User_MapDelete */

/* Start INV_Invitation_User_MapDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_Invitation_User_MapDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_Invitation_User_MapDeleteAll];
GO
CREATE PROCEDURE [dbo].[INV_Invitation_User_MapDeleteAll]
AS
BEGIN
	DELETE [INV_Invitation_User_Map]
END
GO
/* End INV_Invitation_User_MapDeleteAll */
