
/* Start DAM_CompositionProfile_FileGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[CompositionProfileId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_CompositionProfile_File]
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfile_FileGetByID */

/* Start DAM_CompositionProfile_FileGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileGetAll]
AS
BEGIN
	SELECT
		[Id],
		[CompositionProfileId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_CompositionProfile_File]
END
GO
/* End DAM_CompositionProfile_FileGetAll */

/* Start DAM_CompositionProfile_FileGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[CompositionProfileId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_CompositionProfile_File]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_CompositionProfile_FileGetAllOnPage */

/* Start DAM_CompositionProfile_FileInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileInsert];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileInsert]
	@CompositionProfileId int = null,
	@Name nvarchar(100) = null,
	@FileType smallint = null,
	@FileAttach nvarchar(255) = null
AS
BEGIN
	INSERT INTO [DAM_CompositionProfile_File]
	(
		[CompositionProfileId],
		[Name],
		[FileType],
		[FileAttach]
	)
	VALUES
	(
		@CompositionProfileId,
		@Name,
		@FileType,
		@FileAttach
	)
END
GO
/* End DAM_CompositionProfile_FileGetByID */

/* Start DAM_CompositionProfile_FileUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileUpdate]
	@Id int = null,
	@CompositionProfileId int = null,
	@Name nvarchar(100) = null,
	@FileType smallint = null,
	@FileAttach nvarchar(255) = null
AS
BEGIN
	UPDATE [DAM_CompositionProfile_File] SET
		[CompositionProfileId] = @CompositionProfileId,
		[Name] = @Name,
		[FileType] = @FileType,
		[FileAttach] = @FileAttach
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfile_FileUpdate */

/* Start DAM_CompositionProfile_FileDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileDelete];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_CompositionProfile_File]
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfile_FileDelete */

/* Start DAM_CompositionProfile_FileDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfile_FileDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfile_FileDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfile_FileDeleteAll]
AS
BEGIN
	DELETE [DAM_CompositionProfile_File]
END
GO
/* End DAM_CompositionProfile_FileDeleteAll */
