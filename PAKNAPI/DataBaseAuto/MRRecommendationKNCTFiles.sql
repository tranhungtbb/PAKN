
/* Start MR_RecommendationKNCT_FilesGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesGetByID];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationKNCTId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_RecommendationKNCT_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCT_FilesGetByID */

/* Start MR_RecommendationKNCT_FilesGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesGetAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationKNCTId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_RecommendationKNCT_Files]
END
GO
/* End MR_RecommendationKNCT_FilesGetAll */

/* Start MR_RecommendationKNCT_FilesGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationKNCTId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_RecommendationKNCT_Files]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_RecommendationKNCT_FilesGetAllOnPage */

/* Start MR_RecommendationKNCT_FilesInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesInsert];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesInsert]
	@RecommendationKNCTId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	INSERT INTO [MR_RecommendationKNCT_Files]
	(
		[RecommendationKNCTId],
		[Name],
		[FileType],
		[FilePath]
	)
	VALUES
	(
		@RecommendationKNCTId,
		@Name,
		@FileType,
		@FilePath
	)
END
GO
/* End MR_RecommendationKNCT_FilesGetByID */

/* Start MR_RecommendationKNCT_FilesUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesUpdate];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesUpdate]
	@Id int = null,
	@RecommendationKNCTId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	UPDATE [MR_RecommendationKNCT_Files] SET
		[RecommendationKNCTId] = @RecommendationKNCTId,
		[Name] = @Name,
		[FileType] = @FileType,
		[FilePath] = @FilePath
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCT_FilesUpdate */

/* Start MR_RecommendationKNCT_FilesDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesDelete];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_RecommendationKNCT_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCT_FilesDelete */

/* Start MR_RecommendationKNCT_FilesDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCT_FilesDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCT_FilesDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCT_FilesDeleteAll]
AS
BEGIN
	DELETE [MR_RecommendationKNCT_Files]
END
GO
/* End MR_RecommendationKNCT_FilesDeleteAll */
