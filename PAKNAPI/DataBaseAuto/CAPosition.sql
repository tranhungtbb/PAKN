
/* Start CA_PositionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionGetByID];
GO
CREATE PROCEDURE [dbo].[CA_PositionGetByID]
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
	FROM [CA_Position]
	WHERE [Id] = @Id
END
GO
/* End CA_PositionGetByID */

/* Start CA_PositionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionGetAll];
GO
CREATE PROCEDURE [dbo].[CA_PositionGetAll]
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
	FROM [CA_Position]
END
GO
/* End CA_PositionGetAll */

/* Start CA_PositionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_PositionGetAllOnPage]
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
	FROM [CA_Position]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_PositionGetAllOnPage */

/* Start CA_PositionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionInsert];
GO
CREATE PROCEDURE [dbo].[CA_PositionInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber int = null
AS
BEGIN
	INSERT INTO [CA_Position]
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
/* End CA_PositionGetByID */

/* Start CA_PositionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionUpdate];
GO
CREATE PROCEDURE [dbo].[CA_PositionUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber int = null
AS
BEGIN
	UPDATE [CA_Position] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description,
		[OrderNumber] = @OrderNumber
	WHERE [Id] = @Id
END
GO
/* End CA_PositionUpdate */

/* Start CA_PositionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionDelete];
GO
CREATE PROCEDURE [dbo].[CA_PositionDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Position]
	WHERE [Id] = @Id
END
GO
/* End CA_PositionDelete */

/* Start CA_PositionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_PositionDeleteAll]
AS
BEGIN
	DELETE [CA_Position]
END
GO
/* End CA_PositionDeleteAll */

/* Start CA_PositionCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_PositionCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_PositionCount];
GO
CREATE PROCEDURE [dbo].[CA_PositionCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Position]
END
GO
/* End CA_PositionCount */
