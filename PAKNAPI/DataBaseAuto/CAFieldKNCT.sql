
/* Start CA_FieldKNCTGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTGetByID];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name]
	FROM [CA_FieldKNCT]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldKNCTGetByID */

/* Start CA_FieldKNCTGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTGetAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name]
	FROM [CA_FieldKNCT]
END
GO
/* End CA_FieldKNCTGetAll */

/* Start CA_FieldKNCTGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name]
	FROM [CA_FieldKNCT]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_FieldKNCTGetAllOnPage */

/* Start CA_FieldKNCTInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTInsert];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTInsert]
	@Name nvarchar(100) = null
AS
BEGIN
	INSERT INTO [CA_FieldKNCT]
	(
		[Name]
	)
	VALUES
	(
		@Name
	)
END
GO
/* End CA_FieldKNCTGetByID */

/* Start CA_FieldKNCTUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTUpdate];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTUpdate]
	@Id int = null,
	@Name nvarchar(100) = null
AS
BEGIN
	UPDATE [CA_FieldKNCT] SET
		[Name] = @Name
	WHERE [Id] = @Id
END
GO
/* End CA_FieldKNCTUpdate */

/* Start CA_FieldKNCTDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTDelete];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_FieldKNCT]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldKNCTDelete */

/* Start CA_FieldKNCTDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTDeleteAll]
AS
BEGIN
	DELETE [CA_FieldKNCT]
END
GO
/* End CA_FieldKNCTDeleteAll */

/* Start CA_FieldKNCTCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldKNCTCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldKNCTCount];
GO
CREATE PROCEDURE [dbo].[CA_FieldKNCTCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_FieldKNCT]
END
GO
/* End CA_FieldKNCTCount */
