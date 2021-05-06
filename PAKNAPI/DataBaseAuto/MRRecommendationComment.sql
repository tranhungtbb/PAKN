
/* Start MR_Recommendation_CommentGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentGetByID]
	@ID bigint = null
AS
BEGIN
	SELECT
		[ID],
		[UserId],
		[FullName],
		[CreatedDate],
		[CreatedBy],
		[Contents],
		[RecommendationId]
	FROM [MR_Recommendation_Comment]
	WHERE [ID] = @ID
END
GO
/* End MR_Recommendation_CommentGetByID */

/* Start MR_Recommendation_CommentGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentGetAll]
AS
BEGIN
	SELECT
		[ID],
		[UserId],
		[FullName],
		[CreatedDate],
		[CreatedBy],
		[Contents],
		[RecommendationId]
	FROM [MR_Recommendation_Comment]
END
GO
/* End MR_Recommendation_CommentGetAll */

/* Start MR_Recommendation_CommentGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[ID],
		[UserId],
		[FullName],
		[CreatedDate],
		[CreatedBy],
		[Contents],
		[RecommendationId]
	FROM [MR_Recommendation_Comment]
	ORDER BY [ID]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_CommentGetAllOnPage */

/* Start MR_Recommendation_CommentInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentInsert]
	@UserId bigint = null,
	@FullName nvarchar(100) = null,
	@CreatedDate datetime = null,
	@CreatedBy bigint = null,
	@Contents nvarchar(max) = null,
	@RecommendationId bigint = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Comment]
	(
		[UserId],
		[FullName],
		[CreatedDate],
		[CreatedBy],
		[Contents],
		[RecommendationId]
	)
	VALUES
	(
		@UserId,
		@FullName,
		@CreatedDate,
		@CreatedBy,
		@Contents,
		@RecommendationId
	)
END
GO
/* End MR_Recommendation_CommentGetByID */

/* Start MR_Recommendation_CommentUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentUpdate]
	@ID bigint = null,
	@UserId bigint = null,
	@FullName nvarchar(100) = null,
	@CreatedDate datetime = null,
	@CreatedBy bigint = null,
	@Contents nvarchar(max) = null,
	@RecommendationId bigint = null
AS
BEGIN
	UPDATE [MR_Recommendation_Comment] SET
		[UserId] = @UserId,
		[FullName] = @FullName,
		[CreatedDate] = @CreatedDate,
		[CreatedBy] = @CreatedBy,
		[Contents] = @Contents,
		[RecommendationId] = @RecommendationId
	WHERE [ID] = @ID
END
GO
/* End MR_Recommendation_CommentUpdate */

/* Start MR_Recommendation_CommentDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentDelete]
	@ID bigint = null
AS
BEGIN
	DELETE [MR_Recommendation_Comment]
	WHERE [ID] = @ID
END
GO
/* End MR_Recommendation_CommentDelete */

/* Start MR_Recommendation_CommentDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_CommentDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_CommentDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_CommentDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Comment]
END
GO
/* End MR_Recommendation_CommentDeleteAll */
