
/* Start NE_FileAttachGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachGetByID];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[NewsId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [NE_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End NE_FileAttachGetByID */

/* Start NE_FileAttachGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachGetAll];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachGetAll]
AS
BEGIN
	SELECT
		[Id],
		[NewsId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [NE_FileAttach]
END
GO
/* End NE_FileAttachGetAll */

/* Start NE_FileAttachGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[NewsId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [NE_FileAttach]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End NE_FileAttachGetAllOnPage */

/* Start NE_FileAttachInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachInsert];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachInsert]
	@NewsId int = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null,
	@FileType tinyint = null
AS
BEGIN
	INSERT INTO [NE_FileAttach]
	(
		[NewsId],
		[FileAttach],
		[Name],
		[FileType]
	)
	VALUES
	(
		@NewsId,
		@FileAttach,
		@Name,
		@FileType
	)
END
GO
/* End NE_FileAttachGetByID */

/* Start NE_FileAttachUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachUpdate];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachUpdate]
	@Id int = null,
	@NewsId int = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null,
	@FileType tinyint = null
AS
BEGIN
	UPDATE [NE_FileAttach] SET
		[NewsId] = @NewsId,
		[FileAttach] = @FileAttach,
		[Name] = @Name,
		[FileType] = @FileType
	WHERE [Id] = @Id
END
GO
/* End NE_FileAttachUpdate */

/* Start NE_FileAttachDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachDelete];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachDelete]
	@Id int = null
AS
BEGIN
	DELETE [NE_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End NE_FileAttachDelete */

/* Start NE_FileAttachDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachDeleteAll];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachDeleteAll]
AS
BEGIN
	DELETE [NE_FileAttach]
END
GO
/* End NE_FileAttachDeleteAll */

/* Start NE_FileAttachCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_FileAttachCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_FileAttachCount];
GO
CREATE PROCEDURE [dbo].[NE_FileAttachCount]
AS
BEGIN
	SELECT Count(*)
	FROM [NE_FileAttach]
END
GO
/* End NE_FileAttachCount */
