
/* Start MR_Recommendation_FullTextGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[FileId],
		[FullText]
	FROM [MR_Recommendation_FullText]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FullTextGetByID */

/* Start MR_Recommendation_FullTextGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[FileId],
		[FullText]
	FROM [MR_Recommendation_FullText]
END
GO
/* End MR_Recommendation_FullTextGetAll */

/* Start MR_Recommendation_FullTextGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationId],
		[FileId],
		[FullText]
	FROM [MR_Recommendation_FullText]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_FullTextGetAllOnPage */

/* Start MR_Recommendation_FullTextInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextInsert]
	@RecommendationId int = null,
	@FileId int = null,
	@FullText ntext = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_FullText]
	(
		[RecommendationId],
		[FileId],
		[FullText]
	)
	VALUES
	(
		@RecommendationId,
		@FileId,
		@FullText
	)
END
GO
/* End MR_Recommendation_FullTextGetByID */

/* Start MR_Recommendation_FullTextUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextUpdate]
	@Id bigint = null,
	@RecommendationId int = null,
	@FileId int = null,
	@FullText ntext = null
AS
BEGIN
	UPDATE [MR_Recommendation_FullText] SET
		[RecommendationId] = @RecommendationId,
		[FileId] = @FileId,
		[FullText] = @FullText
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FullTextUpdate */

/* Start MR_Recommendation_FullTextDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [MR_Recommendation_FullText]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FullTextDelete */

/* Start MR_Recommendation_FullTextDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FullTextDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FullTextDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FullTextDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_FullText]
END
GO
/* End MR_Recommendation_FullTextDeleteAll */
