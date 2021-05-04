
/* Start INV_FileAttachGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachGetByID];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[InvitationId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [INV_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End INV_FileAttachGetByID */

/* Start INV_FileAttachGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachGetAll];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachGetAll]
AS
BEGIN
	SELECT
		[Id],
		[InvitationId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [INV_FileAttach]
END
GO
/* End INV_FileAttachGetAll */

/* Start INV_FileAttachGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[InvitationId],
		[FileAttach],
		[Name],
		[FileType]
	FROM [INV_FileAttach]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End INV_FileAttachGetAllOnPage */

/* Start INV_FileAttachInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachInsert];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachInsert]
	@InvitationId int = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null,
	@FileType tinyint = null
AS
BEGIN
	INSERT INTO [INV_FileAttach]
	(
		[InvitationId],
		[FileAttach],
		[Name],
		[FileType]
	)
	VALUES
	(
		@InvitationId,
		@FileAttach,
		@Name,
		@FileType
	)
END
GO
/* End INV_FileAttachGetByID */

/* Start INV_FileAttachUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachUpdate];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachUpdate]
	@Id int = null,
	@InvitationId int = null,
	@FileAttach nvarchar(255) = null,
	@Name nvarchar(100) = null,
	@FileType tinyint = null
AS
BEGIN
	UPDATE [INV_FileAttach] SET
		[InvitationId] = @InvitationId,
		[FileAttach] = @FileAttach,
		[Name] = @Name,
		[FileType] = @FileType
	WHERE [Id] = @Id
END
GO
/* End INV_FileAttachUpdate */

/* Start INV_FileAttachDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachDelete];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachDelete]
	@Id int = null
AS
BEGIN
	DELETE [INV_FileAttach]
	WHERE [Id] = @Id
END
GO
/* End INV_FileAttachDelete */

/* Start INV_FileAttachDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[INV_FileAttachDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [INV_FileAttachDeleteAll];
GO
CREATE PROCEDURE [dbo].[INV_FileAttachDeleteAll]
AS
BEGIN
	DELETE [INV_FileAttach]
END
GO
/* End INV_FileAttachDeleteAll */
