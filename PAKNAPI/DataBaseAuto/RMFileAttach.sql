
/* Start RM_FileAttachGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachGetByID];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RemindId],
		[FileType],
		[FileAttach],
		[Name]
	FROM [RM_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End RM_FileAttachGetByID */

/* Start RM_FileAttachGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachGetAll];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RemindId],
		[FileType],
		[FileAttach],
		[Name]
	FROM [RM_FileAttach]
END
GO
/* End RM_FileAttachGetAll */

/* Start RM_FileAttachGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RemindId],
		[FileType],
		[FileAttach],
		[Name]
	FROM [RM_FileAttach]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End RM_FileAttachGetAllOnPage */

/* Start RM_FileAttachInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachInsert];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachInsert]
	@RemindId int = null,
	@FileType tinyint = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null
AS
BEGIN
	INSERT INTO [RM_FileAttach]
	(
		[RemindId],
		[FileType],
		[FileAttach],
		[Name]
	)
	VALUES
	(
		@RemindId,
		@FileType,
		@FileAttach,
		@Name
	)
END
GO
/* End RM_FileAttachGetByID */

/* Start RM_FileAttachUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachUpdate];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachUpdate]
	@Id int = null,
	@RemindId int = null,
	@FileType tinyint = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null
AS
BEGIN
	UPDATE [RM_FileAttach] SET
		[RemindId] = @RemindId,
		[FileType] = @FileType,
		[FileAttach] = @FileAttach,
		[Name] = @Name
	WHERE [Id] = @Id
END
GO
/* End RM_FileAttachUpdate */

/* Start RM_FileAttachDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachDelete];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachDelete]
	@Id int = null
AS
BEGIN
	DELETE [RM_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End RM_FileAttachDelete */

/* Start RM_FileAttachDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_FileAttachDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_FileAttachDeleteAll];
GO
CREATE PROCEDURE [dbo].[RM_FileAttachDeleteAll]
AS
BEGIN
	DELETE [RM_FileAttach]
END
GO
/* End RM_FileAttachDeleteAll */
