
/* Start SY_GroupUserGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_GroupUserGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserGetByID];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[UnitId],
		[CreatedBy],
		[CreatedDate],
		[Description]
	FROM [SY_GroupUser]
	WHERE [Id] = @Id
END
GO
/* End SY_GroupUserGetByID */

/* Start SY_GroupUserGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_GroupUserGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserGetAll];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[UnitId],
		[CreatedBy],
		[CreatedDate],
		[Description]
	FROM [SY_GroupUser]
END
GO
/* End SY_GroupUserGetAll */

/* Start SY_GroupUserGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYGroupUserGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserGetAllOnPage]
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
		[UnitId],
		[CreatedBy],
		[CreatedDate],
		[Description]
	FROM [SY_GroupUser]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_GroupUserGetAllOnPage */

/* Start SY_GroupUserInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYGroupUserInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserInsert];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@UnitId int = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [SY_GroupUser]
	(
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[UnitId],
		[CreatedBy],
		[CreatedDate],
		[Description]
	)
	VALUES
	(
		@Name,
		@Code,
		@IsActived,
		@IsDeleted,
		@UnitId,
		@CreatedBy,
		@CreatedDate,
		@Description
	)
END
GO
/* End SY_GroupUserGetByID */

/* Start SY_GroupUserUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYGroupUserUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserUpdate];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@UnitId int = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [SY_GroupUser] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[UnitId] = @UnitId,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End SY_GroupUserUpdate */

/* Start SY_GroupUserDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYGroupUserDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserDelete];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_GroupUser]
	WHERE [Id] = @Id
END
GO
/* End SY_GroupUserDelete */

/* Start SY_GroupUserDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SYGroupUserDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_GroupUserDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_GroupUserDeleteAll]
AS
BEGIN
	DELETE [SY_GroupUser]
END
GO
/* End SY_GroupUserDeleteAll */
