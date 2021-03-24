
/* Start CA_DepartmentGroupGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupGetByID];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupGetByID]
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
	FROM [CA_DepartmentGroup]
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentGroupGetByID */

/* Start CA_DepartmentGroupGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupGetAll];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_DepartmentGroup]
END
GO
/* End CA_DepartmentGroupGetAll */

/* Start CA_DepartmentGroupGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupGetAllOnPage]
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
	FROM [CA_DepartmentGroup]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_DepartmentGroupGetAllOnPage */

/* Start CA_DepartmentGroupInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupInsert];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [CA_DepartmentGroup]
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
/* End CA_DepartmentGroupGetByID */

/* Start CA_DepartmentGroupUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupUpdate];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [CA_DepartmentGroup] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentGroupUpdate */

/* Start CA_DepartmentGroupDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupDelete];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_DepartmentGroup]
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentGroupDelete */

/* Start CA_DepartmentGroupDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupDeleteAll]
AS
BEGIN
	DELETE [CA_DepartmentGroup]
END
GO
/* End CA_DepartmentGroupDeleteAll */

/* Start CA_DepartmentGroupCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGroupCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGroupCount];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGroupCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_DepartmentGroup]
END
GO
/* End CA_DepartmentGroupCount */
