
/* Start DAM_FileAttachGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End DAM_FileAttachGetByID */

/* Start DAM_FileAttachGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachGetAll]
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_FileAttach]
END
GO
/* End DAM_FileAttachGetAll */

/* Start DAM_FileAttachGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[AdministrationId],
		[Name],
		[FileType],
		[FileAttach]
	FROM [DAM_FileAttach]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_FileAttachGetAllOnPage */

/* Start DAM_FileAttachInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachInsert];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachInsert]
	@AdministrationId int = null,
	@Name nvarchar(100) = null,
	@FileType smallint = null,
	@FileAttach nvarchar(255) = null
AS
BEGIN
	INSERT INTO [DAM_FileAttach]
	(
		[AdministrationId],
		[Name],
		[FileType],
		[FileAttach]
	)
	VALUES
	(
		@AdministrationId,
		@Name,
		@FileType,
		@FileAttach
	)
END
GO
/* End DAM_FileAttachGetByID */

/* Start DAM_FileAttachUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachUpdate]
	@Id int = null,
	@AdministrationId int = null,
	@Name nvarchar(100) = null,
	@FileType smallint = null,
	@FileAttach nvarchar(255) = null
AS
BEGIN
	UPDATE [DAM_FileAttach] SET
		[AdministrationId] = @AdministrationId,
		[Name] = @Name,
		[FileType] = @FileType,
		[FileAttach] = @FileAttach
	WHERE [Id] = @Id
END
GO
/* End DAM_FileAttachUpdate */

/* Start DAM_FileAttachDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachDelete];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End DAM_FileAttachDelete */

/* Start DAM_FileAttachDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_FileAttachDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_FileAttachDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_FileAttachDeleteAll]
AS
BEGIN
	DELETE [DAM_FileAttach]
END
GO
/* End DAM_FileAttachDeleteAll */
