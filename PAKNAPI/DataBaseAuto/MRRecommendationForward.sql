
/* Start MR_Recommendation_ForwardGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Status],
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Step],
		[Content],
		[ReasonDeny],
		[SendDate],
		[ExpiredDate],
		[ProcessingDate],
		[IsViewed]
	FROM [MR_Recommendation_Forward]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ForwardGetByID */

/* Start MR_Recommendation_ForwardGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardGetAll]
AS
BEGIN
	SELECT
		[Status],
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Step],
		[Content],
		[ReasonDeny],
		[SendDate],
		[ExpiredDate],
		[ProcessingDate],
		[IsViewed]
	FROM [MR_Recommendation_Forward]
END
GO
/* End MR_Recommendation_ForwardGetAll */

/* Start MR_Recommendation_ForwardGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Status],
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Step],
		[Content],
		[ReasonDeny],
		[SendDate],
		[ExpiredDate],
		[ProcessingDate],
		[IsViewed]
	FROM [MR_Recommendation_Forward]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_ForwardGetAllOnPage */

/* Start MR_Recommendation_ForwardInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardInsert]
	@Status tinyint = null,
	@RecommendationId int = null,
	@UserSendId bigint = null,
	@UnitSendId int = null,
	@ReceiveId bigint = null,
	@UnitReceiveId int = null,
	@Step tinyint = null,
	@Content nvarchar(500) = null,
	@ReasonDeny nvarchar(500) = null,
	@SendDate datetime = null,
	@ExpiredDate datetime = null,
	@ProcessingDate datetime = null,
	@IsViewed bit = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Forward]
	(
		[Status],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Step],
		[Content],
		[ReasonDeny],
		[SendDate],
		[ExpiredDate],
		[ProcessingDate],
		[IsViewed]
	)
	VALUES
	(
		@Status,
		@RecommendationId,
		@UserSendId,
		@UnitSendId,
		@ReceiveId,
		@UnitReceiveId,
		@Step,
		@Content,
		@ReasonDeny,
		@SendDate,
		@ExpiredDate,
		@ProcessingDate,
		@IsViewed
	)
END
GO
/* End MR_Recommendation_ForwardGetByID */

/* Start MR_Recommendation_ForwardUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardUpdate]
	@Status tinyint = null,
	@Id int = null,
	@RecommendationId int = null,
	@UserSendId bigint = null,
	@UnitSendId int = null,
	@ReceiveId bigint = null,
	@UnitReceiveId int = null,
	@Step tinyint = null,
	@Content nvarchar(500) = null,
	@ReasonDeny nvarchar(500) = null,
	@SendDate datetime = null,
	@ExpiredDate datetime = null,
	@ProcessingDate datetime = null,
	@IsViewed bit = null
AS
BEGIN
	UPDATE [MR_Recommendation_Forward] SET
		[Status] = @Status,
		[RecommendationId] = @RecommendationId,
		[UserSendId] = @UserSendId,
		[UnitSendId] = @UnitSendId,
		[ReceiveId] = @ReceiveId,
		[UnitReceiveId] = @UnitReceiveId,
		[Step] = @Step,
		[Content] = @Content,
		[ReasonDeny] = @ReasonDeny,
		[SendDate] = @SendDate,
		[ExpiredDate] = @ExpiredDate,
		[ProcessingDate] = @ProcessingDate,
		[IsViewed] = @IsViewed
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ForwardUpdate */

/* Start MR_Recommendation_ForwardDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_Forward]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ForwardDelete */

/* Start MR_Recommendation_ForwardDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ForwardDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ForwardDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ForwardDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Forward]
END
GO
/* End MR_Recommendation_ForwardDeleteAll */
