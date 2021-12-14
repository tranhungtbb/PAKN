
/* Start MR_Recommendation_GroupWordGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[GroupWordId],
		[GroupWordName]
	FROM [MR_Recommendation_GroupWord]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GroupWordGetByID */

/* Start MR_Recommendation_GroupWordGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[GroupWordId],
		[GroupWordName]
	FROM [MR_Recommendation_GroupWord]
END
GO
/* End MR_Recommendation_GroupWordGetAll */

/* Start MR_Recommendation_GroupWordGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationId],
		[GroupWordId],
		[GroupWordName]
	FROM [MR_Recommendation_GroupWord]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_GroupWordGetAllOnPage */

/* Start MR_Recommendation_GroupWordInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordInsert]
	@RecommendationId int = null,
	@GroupWordId int = null,
	@GroupWordName nvarchar(100) = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_GroupWord]
	(
		[RecommendationId],
		[GroupWordId],
		[GroupWordName]
	)
	VALUES
	(
		@RecommendationId,
		@GroupWordId,
		@GroupWordName
	)
END
GO
/* End MR_Recommendation_GroupWordGetByID */

/* Start MR_Recommendation_GroupWordUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordUpdate]
	@Id int = null,
	@RecommendationId int = null,
	@GroupWordId int = null,
	@GroupWordName nvarchar(100) = null
AS
BEGIN
	UPDATE [MR_Recommendation_GroupWord] SET
		[RecommendationId] = @RecommendationId,
		[GroupWordId] = @GroupWordId,
		[GroupWordName] = @GroupWordName
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GroupWordUpdate */

/* Start MR_Recommendation_GroupWordDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_GroupWord]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GroupWordDelete */

/* Start MR_Recommendation_GroupWordDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GroupWordDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GroupWordDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GroupWordDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_GroupWord]
END
GO
/* End MR_Recommendation_GroupWordDeleteAll */
