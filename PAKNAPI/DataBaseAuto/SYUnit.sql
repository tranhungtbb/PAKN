
/* Start SY_UnitGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitGetByID];
GO
CREATE PROCEDURE [dbo].[SY_UnitGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[UnitLevel],
		[IsActived],
		[IsDeleted],
		[IsMain],
		[ParentId],
		[Description],
		[Email],
		[Phone],
		[Address]
	FROM [SY_Unit]
	WHERE [Id] = @Id
END
GO
/* End SY_UnitGetByID */

/* Start SY_UnitGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitGetAll];
GO
CREATE PROCEDURE [dbo].[SY_UnitGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[UnitLevel],
		[IsActived],
		[IsDeleted],
		[IsMain],
		[ParentId],
		[Description],
		[Email],
		[Phone],
		[Address]
	FROM [SY_Unit]
END
GO
/* End SY_UnitGetAll */

/* Start SY_UnitGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_UnitGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[UnitLevel],
		[IsActived],
		[IsDeleted],
		[IsMain],
		[ParentId],
		[Description],
		[Email],
		[Phone],
		[Address]
	FROM [SY_Unit]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_UnitGetAllOnPage */

/* Start SY_UnitInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitInsert];
GO
CREATE PROCEDURE [dbo].[SY_UnitInsert]
	@Name nvarchar(500) = null,
	@UnitLevel tinyint = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@IsMain bit = null,
	@ParentId int = null,
	@Description nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Address nvarchar(500) = null
AS
BEGIN
	INSERT INTO [SY_Unit]
	(
		[Name],
		[UnitLevel],
		[IsActived],
		[IsDeleted],
		[IsMain],
		[ParentId],
		[Description],
		[Email],
		[Phone],
		[Address]
	)
	VALUES
	(
		@Name,
		@UnitLevel,
		@IsActived,
		@IsDeleted,
		@IsMain,
		@ParentId,
		@Description,
		@Email,
		@Phone,
		@Address
	)
END
GO
/* End SY_UnitGetByID */

/* Start SY_UnitUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitUpdate];
GO
CREATE PROCEDURE [dbo].[SY_UnitUpdate]
	@Id int = null,
	@Name nvarchar(500) = null,
	@UnitLevel tinyint = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@IsMain bit = null,
	@ParentId int = null,
	@Description nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Address nvarchar(500) = null
AS
BEGIN
	UPDATE [SY_Unit] SET
		[Name] = @Name,
		[UnitLevel] = @UnitLevel,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[IsMain] = @IsMain,
		[ParentId] = @ParentId,
		[Description] = @Description,
		[Email] = @Email,
		[Phone] = @Phone,
		[Address] = @Address
	WHERE [Id] = @Id
END
GO
/* End SY_UnitUpdate */

/* Start SY_UnitDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitDelete];
GO
CREATE PROCEDURE [dbo].[SY_UnitDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_Unit]
	WHERE [Id] = @Id
END
GO
/* End SY_UnitDelete */

/* Start SY_UnitDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_UnitDeleteAll]
AS
BEGIN
	DELETE [SY_Unit]
END
GO
/* End SY_UnitDeleteAll */

/* Start SY_UnitCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UnitCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UnitCount];
GO
CREATE PROCEDURE [dbo].[SY_UnitCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Unit]
END
GO
/* End SY_UnitCount */
