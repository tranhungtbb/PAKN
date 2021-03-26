
/* Start CA_NewsTypeGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeGetByID];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_NewsType]
	WHERE [Id] = @Id
END
GO
/* End CA_NewsTypeGetByID */

/* Start CA_NewsTypeGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeGetAll];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_NewsType]
END
GO
/* End CA_NewsTypeGetAll */

/* Start CA_NewsTypeGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_NewsType]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_NewsTypeGetAllOnPage */

/* Start CA_NewsTypeInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeInsert];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeInsert]
	@Name nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [CA_NewsType]
	(
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	)
	VALUES
	(
		@Name,
		@IsActived,
		@IsDeleted,
		@Description
	)
END
GO
/* End CA_NewsTypeGetByID */

/* Start CA_NewsTypeUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeUpdate];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [CA_NewsType] SET
		[Name] = @Name,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End CA_NewsTypeUpdate */

/* Start CA_NewsTypeDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeDelete];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_NewsType]
	WHERE [Id] = @Id
END
GO
/* End CA_NewsTypeDelete */

/* Start CA_NewsTypeDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeDeleteAll]
AS
BEGIN
	DELETE [CA_NewsType]
END
GO
/* End CA_NewsTypeDeleteAll */

/* Start CA_NewsTypeCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_NewsTypeCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_NewsTypeCount];
GO
CREATE PROCEDURE [dbo].[CA_NewsTypeCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_NewsType]
END
GO
/* End CA_NewsTypeCount */
