
/* Start CA_WordGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordGetByID];
GO
CREATE PROCEDURE [dbo].[CA_WordGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_Word]
	WHERE [Id] = @Id
END
GO
/* End CA_WordGetByID */

/* Start CA_WordGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordGetAll];
GO
CREATE PROCEDURE [dbo].[CA_WordGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_Word]
END
GO
/* End CA_WordGetAll */

/* Start CA_WordGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_WordGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_Word]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_WordGetAllOnPage */

/* Start CA_WordInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordInsert];
GO
CREATE PROCEDURE [dbo].[CA_WordInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [CA_Word]
	(
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description]
	)
	VALUES
	(
		@Name,
		@Code,
		@IsActived,
		@IsDeleted,
		@Description
	)
END
GO
/* End CA_WordGetByID */

/* Start CA_WordUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordUpdate];
GO
CREATE PROCEDURE [dbo].[CA_WordUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [CA_Word] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End CA_WordUpdate */

/* Start CA_WordDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordDelete];
GO
CREATE PROCEDURE [dbo].[CA_WordDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Word]
	WHERE [Id] = @Id
END
GO
/* End CA_WordDelete */

/* Start CA_WordDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_WordDeleteAll]
AS
BEGIN
	DELETE [CA_Word]
END
GO
/* End CA_WordDeleteAll */

/* Start CA_WordCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WordCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WordCount];
GO
CREATE PROCEDURE [dbo].[CA_WordCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Word]
END
GO
/* End CA_WordCount */
