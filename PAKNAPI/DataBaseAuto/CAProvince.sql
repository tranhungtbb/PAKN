
/* Start CA_ProvinceGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceGetByID];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Province]
	WHERE [Id] = @Id
END
GO
/* End CA_ProvinceGetByID */

/* Start CA_ProvinceGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceGetAll];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Province]
END
GO
/* End CA_ProvinceGetAll */

/* Start CA_ProvinceGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Province]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_ProvinceGetAllOnPage */

/* Start CA_ProvinceInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceInsert];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceInsert]
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	INSERT INTO [CA_Province]
	(
		[Name],
		[IsActived],
		[IsDeleted]
	)
	VALUES
	(
		@Name,
		@IsActived,
		@IsDeleted
	)
END
GO
/* End CA_ProvinceGetByID */

/* Start CA_ProvinceUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceUpdate];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceUpdate]
	@Id int = null,
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	UPDATE [CA_Province] SET
		[Name] = @Name,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted
	WHERE [Id] = @Id
END
GO
/* End CA_ProvinceUpdate */

/* Start CA_ProvinceDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceDelete];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Province]
	WHERE [Id] = @Id
END
GO
/* End CA_ProvinceDelete */

/* Start CA_ProvinceDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceDeleteAll]
AS
BEGIN
	DELETE [CA_Province]
END
GO
/* End CA_ProvinceDeleteAll */

/* Start CA_ProvinceCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_ProvinceCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_ProvinceCount];
GO
CREATE PROCEDURE [dbo].[CA_ProvinceCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Province]
END
GO
/* End CA_ProvinceCount */
