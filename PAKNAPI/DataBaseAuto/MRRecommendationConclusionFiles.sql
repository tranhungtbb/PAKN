
/* Start MR_Recommendation_Conclusion_FilesGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[ConclusionId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Conclusion_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_Conclusion_FilesGetByID */

/* Start MR_Recommendation_Conclusion_FilesGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesGetAll]
AS
BEGIN
	SELECT
		[Id],
		[ConclusionId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Conclusion_Files]
END
GO
/* End MR_Recommendation_Conclusion_FilesGetAll */

/* Start MR_Recommendation_Conclusion_FilesGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[ConclusionId],
		[Name],
		[FileType],
		[FilePath]
	FROM [MR_Recommendation_Conclusion_Files]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_Conclusion_FilesGetAllOnPage */

/* Start MR_Recommendation_Conclusion_FilesInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesInsert]
	@ConclusionId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Conclusion_Files]
	(
		[ConclusionId],
		[Name],
		[FileType],
		[FilePath]
	)
	VALUES
	(
		@ConclusionId,
		@Name,
		@FileType,
		@FilePath
	)
END
GO
/* End MR_Recommendation_Conclusion_FilesGetByID */

/* Start MR_Recommendation_Conclusion_FilesUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesUpdate]
	@Id int = null,
	@ConclusionId int = null,
	@Name nvarchar(255) = null,
	@FileType smallint = null,
	@FilePath nvarchar(500) = null
AS
BEGIN
	UPDATE [MR_Recommendation_Conclusion_Files] SET
		[ConclusionId] = @ConclusionId,
		[Name] = @Name,
		[FileType] = @FileType,
		[FilePath] = @FilePath
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_Conclusion_FilesUpdate */

/* Start MR_Recommendation_Conclusion_FilesDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_Conclusion_Files]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_Conclusion_FilesDelete */

/* Start MR_Recommendation_Conclusion_FilesDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Conclusion_Files]
END
GO
/* End MR_Recommendation_Conclusion_FilesDeleteAll */

/* Start MR_Recommendation_Conclusion_FilesCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_Conclusion_FilesCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_Conclusion_FilesCount];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_Conclusion_FilesCount]
AS
BEGIN
	SELECT Count(*)
	FROM [MR_Recommendation_Conclusion_Files]
END
GO
/* End MR_Recommendation_Conclusion_FilesCount */
