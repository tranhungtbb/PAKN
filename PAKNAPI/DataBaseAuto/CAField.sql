
/* Start CA_FieldGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldGetByID];
GO
CREATE PROCEDURE [dbo].[CA_FieldGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description],
		[OrderNumber]
	FROM [CA_Field]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldGetByID */

/* Start CA_FieldGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldGetAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description],
		[OrderNumber]
	FROM [CA_Field]
END
GO
/* End CA_FieldGetAll */

/* Start CA_FieldGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_FieldGetAllOnPage]
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
		[Description],
		[OrderNumber]
	FROM [CA_Field]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_FieldGetAllOnPage */

/* Start CA_FieldInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldInsert];
GO
CREATE PROCEDURE [dbo].[CA_FieldInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber int = null
AS
BEGIN
	INSERT INTO [CA_Field]
	(
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description],
		[OrderNumber]
	)
	VALUES
	(
		@Name,
		@Code,
		@IsActived,
		@IsDeleted,
		@Description,
		@OrderNumber
	)
END
GO
/* End CA_FieldGetByID */

/* Start CA_FieldUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldUpdate];
GO
CREATE PROCEDURE [dbo].[CA_FieldUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber int = null
AS
BEGIN
	UPDATE [CA_Field] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description,
		[OrderNumber] = @OrderNumber
	WHERE [Id] = @Id
END
GO
/* End CA_FieldUpdate */

/* Start CA_FieldDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDelete];
GO
CREATE PROCEDURE [dbo].[CA_FieldDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Field]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldDelete */

/* Start CA_FieldDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldDeleteAll]
AS
BEGIN
	DELETE [CA_Field]
END
GO
/* End CA_FieldDeleteAll */

/* Start CA_FieldCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldCount];
GO
CREATE PROCEDURE [dbo].[CA_FieldCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Field]
END
GO
/* End CA_FieldCount */
