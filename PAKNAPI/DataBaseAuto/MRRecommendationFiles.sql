
/* Start MR_Recommendation_FilesGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FilesGetByID */

/* Start MR_Recommendation_FilesGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Files]
END
GO
/* End MR_Recommendation_FilesGetAll */

/* Start MR_Recommendation_FilesGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Files]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_FilesGetAllOnPage */

/* Start MR_Recommendation_FilesInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesInsert]
	@RecommendationId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Files]
	(
		[RecommendationId],
		[Name],
		[FileType],
		[FilePath]
	)
	VALUES
	(
		@RecommendationId,
		@Name,
		@FileType,
		@FilePath
	)
END
GO
/* End MR_Recommendation_FilesGetByID */

/* Start MR_Recommendation_FilesUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesUpdate]
	@Id int = null,
	@RecommendationId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	UPDATE [MR_Recommendation_Files] SET
		[RecommendationId] = @RecommendationId,
		[Name] = @Name,
		[FileType] = @FileType,
		[FilePath] = @FilePath
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FilesUpdate */

/* Start MR_Recommendation_FilesDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_FilesDelete */

/* Start MR_Recommendation_FilesDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_FilesDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_FilesDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_FilesDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Files]
END
GO
/* End MR_Recommendation_FilesDeleteAll */
