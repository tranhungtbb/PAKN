
/* Start SY_RoleGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleGetByID];
GO
CREATE PROCEDURE [dbo].[SY_RoleGetByID]
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
	FROM [SY_Role]
	WHERE [Id] = @Id
END
GO
/* End SY_RoleGetByID */

/* Start SY_RoleGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleGetAll];
GO
CREATE PROCEDURE [dbo].[SY_RoleGetAll]
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
	FROM [SY_Role]
END
GO
/* End SY_RoleGetAll */

/* Start SY_RoleGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_RoleGetAllOnPage]
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
	FROM [SY_Role]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_RoleGetAllOnPage */

/* Start SY_RoleInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleInsert];
GO
CREATE PROCEDURE [dbo].[SY_RoleInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber nchar(10) = null
AS
BEGIN
	INSERT INTO [SY_Role]
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
/* End SY_RoleGetByID */

/* Start SY_RoleUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleUpdate];
GO
CREATE PROCEDURE [dbo].[SY_RoleUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null,
	@OrderNumber nchar(10) = null
AS
BEGIN
	UPDATE [SY_Role] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description,
		[OrderNumber] = @OrderNumber
	WHERE [Id] = @Id
END
GO
/* End SY_RoleUpdate */

/* Start SY_RoleDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleDelete];
GO
CREATE PROCEDURE [dbo].[SY_RoleDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_Role]
	WHERE [Id] = @Id
END
GO
/* End SY_RoleDelete */

/* Start SY_RoleDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_RoleDeleteAll]
AS
BEGIN
	DELETE [SY_Role]
END
GO
/* End SY_RoleDeleteAll */

/* Start SY_RoleCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_RoleCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_RoleCount];
GO
CREATE PROCEDURE [dbo].[SY_RoleCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Role]
END
GO
/* End SY_RoleCount */
