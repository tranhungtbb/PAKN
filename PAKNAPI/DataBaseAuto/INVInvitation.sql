
/* Start INV_InvitationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationGetByID];
GO
CREATE PROCEDURE [dbo].[INV_InvitationGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Title],
		[StartDate],
		[EndDate],
		[Content],
		[Place],
		[CreateDate],
		[UserCreateId],
		[Status],
		[IsView],
		[Member],
		[UpdateDate],
		[UserUpdate],
		[SendDate],
		[Note]
	FROM [INV_Invitation]
	WHERE [Id] = @Id
END
GO
/* End INV_InvitationGetByID */

/* Start INV_InvitationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationGetAll];
GO
CREATE PROCEDURE [dbo].[INV_InvitationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Title],
		[StartDate],
		[EndDate],
		[Content],
		[Place],
		[CreateDate],
		[UserCreateId],
		[Status],
		[IsView],
		[Member],
		[UpdateDate],
		[UserUpdate],
		[SendDate],
		[Note]
	FROM [INV_Invitation]
END
GO
/* End INV_InvitationGetAll */

/* Start INV_InvitationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[INV_InvitationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Title],
		[StartDate],
		[EndDate],
		[Content],
		[Place],
		[CreateDate],
		[UserCreateId],
		[Status],
		[IsView],
		[Member],
		[UpdateDate],
		[UserUpdate],
		[SendDate],
		[Note]
	FROM [INV_Invitation]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End INV_InvitationGetAllOnPage */

/* Start INV_InvitationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationInsert];
GO
CREATE PROCEDURE [dbo].[INV_InvitationInsert]
	@Title nvarchar(1000) = null,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@Content nvarchar(1000) = null,
	@Place nvarchar(1000) = null,
	@CreateDate datetime = null,
	@UserCreateId int = null,
	@Status tinyint = null,
	@IsView bigint = null,
	@Member int = null,
	@UpdateDate datetime = null,
	@UserUpdate int = null,
	@SendDate datetime = null,
	@Note nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [INV_Invitation]
	(
		[Title],
		[StartDate],
		[EndDate],
		[Content],
		[Place],
		[CreateDate],
		[UserCreateId],
		[Status],
		[IsView],
		[Member],
		[UpdateDate],
		[UserUpdate],
		[SendDate],
		[Note]
	)
	VALUES
	(
		@Title,
		@StartDate,
		@EndDate,
		@Content,
		@Place,
		@CreateDate,
		@UserCreateId,
		@Status,
		@IsView,
		@Member,
		@UpdateDate,
		@UserUpdate,
		@SendDate,
		@Note
	)
END
GO
/* End INV_InvitationGetByID */

/* Start INV_InvitationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationUpdate];
GO
CREATE PROCEDURE [dbo].[INV_InvitationUpdate]
	@Id int = null,
	@Title nvarchar(1000) = null,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@Content nvarchar(1000) = null,
	@Place nvarchar(1000) = null,
	@CreateDate datetime = null,
	@UserCreateId int = null,
	@Status tinyint = null,
	@IsView bigint = null,
	@Member int = null,
	@UpdateDate datetime = null,
	@UserUpdate int = null,
	@SendDate datetime = null,
	@Note nvarchar(1000) = null
AS
BEGIN
	UPDATE [INV_Invitation] SET
		[Title] = @Title,
		[StartDate] = @StartDate,
		[EndDate] = @EndDate,
		[Content] = @Content,
		[Place] = @Place,
		[CreateDate] = @CreateDate,
		[UserCreateId] = @UserCreateId,
		[Status] = @Status,
		[IsView] = @IsView,
		[Member] = @Member,
		[UpdateDate] = @UpdateDate,
		[UserUpdate] = @UserUpdate,
		[SendDate] = @SendDate,
		[Note] = @Note
	WHERE [Id] = @Id
END
GO
/* End INV_InvitationUpdate */

/* Start INV_InvitationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationDelete];
GO
CREATE PROCEDURE [dbo].[INV_InvitationDelete]
	@Id int = null
AS
BEGIN
	DELETE [INV_Invitation]
	WHERE [Id] = @Id
END
GO
/* End INV_InvitationDelete */

/* Start INV_InvitationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_InvitationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_InvitationDeleteAll];
GO
CREATE PROCEDURE [dbo].[INV_InvitationDeleteAll]
AS
BEGIN
	DELETE [INV_Invitation]
END
GO
/* End INV_InvitationDeleteAll */
