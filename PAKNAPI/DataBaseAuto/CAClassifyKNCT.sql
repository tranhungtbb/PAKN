
/* Start CA_ClassifyKNCTGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTGetByID];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name]
	FROM [CA_ClassifyKNCT]
	WHERE [Id] = @Id
END
GO
/* End CA_ClassifyKNCTGetByID */

/* Start CA_ClassifyKNCTGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTGetAll];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name]
	FROM [CA_ClassifyKNCT]
END
GO
/* End CA_ClassifyKNCTGetAll */

/* Start CA_ClassifyKNCTGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name]
	FROM [CA_ClassifyKNCT]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_ClassifyKNCTGetAllOnPage */

/* Start CA_ClassifyKNCTInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTInsert];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTInsert]
	@Name nvarchar(100) = null
AS
BEGIN
	INSERT INTO [CA_ClassifyKNCT]
	(
		[Name]
	)
	VALUES
	(
		@Name
	)
END
GO
/* End CA_ClassifyKNCTGetByID */

/* Start CA_ClassifyKNCTUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTUpdate];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTUpdate]
	@Id int = null,
	@Name nvarchar(100) = null
AS
BEGIN
	UPDATE [CA_ClassifyKNCT] SET
		[Name] = @Name
	WHERE [Id] = @Id
END
GO
/* End CA_ClassifyKNCTUpdate */

/* Start CA_ClassifyKNCTDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTDelete];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_ClassifyKNCT]
	WHERE [Id] = @Id
END
GO
/* End CA_ClassifyKNCTDelete */

/* Start CA_ClassifyKNCTDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTDeleteAll]
AS
BEGIN
	DELETE [CA_ClassifyKNCT]
END
GO
/* End CA_ClassifyKNCTDeleteAll */

/* Start CA_ClassifyKNCTCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ClassifyKNCTCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ClassifyKNCTCount];
GO
CREATE PROCEDURE [dbo].[CA_ClassifyKNCTCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_ClassifyKNCT]
END
GO
/* End CA_ClassifyKNCTCount */
