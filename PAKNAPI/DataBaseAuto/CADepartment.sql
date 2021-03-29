
/* Start CA_DepartmentGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGetByID];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Phone],
		[Description],
		[Address],
		[Fax],
		[Email],
		[DepartmentGroupId]
	FROM [CA_Department]
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentGetByID */

/* Start CA_DepartmentGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGetAll];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Phone],
		[Description],
		[Address],
		[Fax],
		[Email],
		[DepartmentGroupId]
	FROM [CA_Department]
END
GO
/* End CA_DepartmentGetAll */

/* Start CA_DepartmentGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentGetAllOnPage]
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
		[Phone],
		[Description],
		[Address],
		[Fax],
		[Email],
		[DepartmentGroupId]
	FROM [CA_Department]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_DepartmentGetAllOnPage */

/* Start CA_DepartmentInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentInsert];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentInsert]
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Phone nvarchar(50) = null,
	@Description nvarchar(1000) = null,
	@Address nvarchar(200) = null,
	@Fax nvarchar(50) = null,
	@Email nvarchar(50) = null,
	@DepartmentGroupId int = null
AS
BEGIN
	INSERT INTO [CA_Department]
	(
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Phone],
		[Description],
		[Address],
		[Fax],
		[Email],
		[DepartmentGroupId]
	)
	VALUES
	(
		@Name,
		@Code,
		@IsActived,
		@IsDeleted,
		@Phone,
		@Description,
		@Address,
		@Fax,
		@Email,
		@DepartmentGroupId
	)
END
GO
/* End CA_DepartmentGetByID */

/* Start CA_DepartmentUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentUpdate];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@Code nvarchar(50) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Phone nvarchar(50) = null,
	@Description nvarchar(1000) = null,
	@Address nvarchar(200) = null,
	@Fax nvarchar(50) = null,
	@Email nvarchar(50) = null,
	@DepartmentGroupId int = null
AS
BEGIN
	UPDATE [CA_Department] SET
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Phone] = @Phone,
		[Description] = @Description,
		[Address] = @Address,
		[Fax] = @Fax,
		[Email] = @Email,
		[DepartmentGroupId] = @DepartmentGroupId
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentUpdate */

/* Start CA_DepartmentDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentDelete];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Department]
	WHERE [Id] = @Id
END
GO
/* End CA_DepartmentDelete */

/* Start CA_DepartmentDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentDeleteAll]
AS
BEGIN
	DELETE [CA_Department]
END
GO
/* End CA_DepartmentDeleteAll */

/* Start CA_DepartmentCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DepartmentCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DepartmentCount];
GO
CREATE PROCEDURE [dbo].[CA_DepartmentCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Department]
END
GO
/* End CA_DepartmentCount */
