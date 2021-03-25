
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
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Status],
		[Content],
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
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Status],
		[Content],
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
		[Id],
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Status],
		[Content],
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
	@RecommendationId int = null,
	@UserSendId int = null,
	@UnitSendId int = null,
	@ReceiveId int = null,
	@UnitReceiveId int = null,
	@Status tinyint = null,
	@Content nvarchar(max) = null,
	@SendDate datetime = null,
	@ExpiredDate datetime = null,
	@ProcessingDate datetime = null,
	@IsViewed bit = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Forward]
	(
		[RecommendationId],
		[UserSendId],
		[UnitSendId],
		[ReceiveId],
		[UnitReceiveId],
		[Status],
		[Content],
		[SendDate],
		[ExpiredDate],
		[ProcessingDate],
		[IsViewed]
	)
	VALUES
	(
		@RecommendationId,
		@UserSendId,
		@UnitSendId,
		@ReceiveId,
		@UnitReceiveId,
		@Status,
		@Content,
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
	@Id int = null,
	@RecommendationId int = null,
	@UserSendId int = null,
	@UnitSendId int = null,
	@ReceiveId int = null,
	@UnitReceiveId int = null,
	@Status tinyint = null,
	@Content nvarchar(max) = null,
	@SendDate datetime = null,
	@ExpiredDate datetime = null,
	@ProcessingDate datetime = null,
	@IsViewed bit = null
AS
BEGIN
	UPDATE [MR_Recommendation_Forward] SET
		[RecommendationId] = @RecommendationId,
		[UserSendId] = @UserSendId,
		[UnitSendId] = @UnitSendId,
		[ReceiveId] = @ReceiveId,
		[UnitReceiveId] = @UnitReceiveId,
		[Status] = @Status,
		[Content] = @Content,
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
