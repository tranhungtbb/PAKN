
/* Start MR_Recommendation_HashtagGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[HashtagId],
		[HashtagName]
	FROM [MR_Recommendation_Hashtag]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_HashtagGetByID */

/* Start MR_Recommendation_HashtagGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[HashtagId],
		[HashtagName]
	FROM [MR_Recommendation_Hashtag]
END
GO
/* End MR_Recommendation_HashtagGetAll */

/* Start MR_Recommendation_HashtagGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationId],
		[HashtagId],
		[HashtagName]
	FROM [MR_Recommendation_Hashtag]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_HashtagGetAllOnPage */

/* Start MR_Recommendation_HashtagInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagInsert]
	@RecommendationId int = null,
	@HashtagId int = null,
	@HashtagName nvarchar(50) = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Hashtag]
	(
		[RecommendationId],
		[HashtagId],
		[HashtagName]
	)
	VALUES
	(
		@RecommendationId,
		@HashtagId,
		@HashtagName
	)
END
GO
/* End MR_Recommendation_HashtagGetByID */

/* Start MR_Recommendation_HashtagUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagUpdate]
	@Id bigint = null,
	@RecommendationId int = null,
	@HashtagId int = null,
	@HashtagName nvarchar(50) = null
AS
BEGIN
	UPDATE [MR_Recommendation_Hashtag] SET
		[RecommendationId] = @RecommendationId,
		[HashtagId] = @HashtagId,
		[HashtagName] = @HashtagName
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_HashtagUpdate */

/* Start MR_Recommendation_HashtagDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [MR_Recommendation_Hashtag]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_HashtagDelete */

/* Start MR_Recommendation_HashtagDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_HashtagDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_HashtagDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_HashtagDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Hashtag]
END
GO
/* End MR_Recommendation_HashtagDeleteAll */
