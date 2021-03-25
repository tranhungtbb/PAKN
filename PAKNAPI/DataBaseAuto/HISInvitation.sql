
/* Start HIS_InvitationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Invitation]
	WHERE [Id] = @Id
END
GO
/* End HIS_InvitationGetByID */

/* Start HIS_InvitationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Invitation]
END
GO
/* End HIS_InvitationGetAll */

/* Start HIS_InvitationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Invitation]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_InvitationGetAllOnPage */

/* Start HIS_InvitationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationInsert];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_Invitation]
	(
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	)
	VALUES
	(
		@ObjectId,
		@Type,
		@Content,
		@Status,
		@CreatedBy,
		@CreatedDate
	)
END
GO
/* End HIS_InvitationGetByID */

/* Start HIS_InvitationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_Invitation] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_InvitationUpdate */

/* Start HIS_InvitationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationDelete];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_Invitation]
	WHERE [Id] = @Id
END
GO
/* End HIS_InvitationDelete */

/* Start HIS_InvitationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_InvitationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_InvitationDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_InvitationDeleteAll]
AS
BEGIN
	DELETE [HIS_Invitation]
END
GO
/* End HIS_InvitationDeleteAll */
