
/* Start CA_WardsGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsGetByID];
GO
CREATE PROCEDURE [dbo].[CA_WardsGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[DistrictId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Wards]
	WHERE [Id] = @Id
END
GO
/* End CA_WardsGetByID */

/* Start CA_WardsGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsGetAll];
GO
CREATE PROCEDURE [dbo].[CA_WardsGetAll]
AS
BEGIN
	SELECT
		[Id],
		[DistrictId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Wards]
END
GO
/* End CA_WardsGetAll */

/* Start CA_WardsGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_WardsGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[DistrictId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_Wards]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_WardsGetAllOnPage */

/* Start CA_WardsInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsInsert];
GO
CREATE PROCEDURE [dbo].[CA_WardsInsert]
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	INSERT INTO [CA_Wards]
	(
		[DistrictId],
		[Name],
		[IsActived],
		[IsDeleted]
	)
	VALUES
	(
		@DistrictId,
		@Name,
		@IsActived,
		@IsDeleted
	)
END
GO
/* End CA_WardsGetByID */

/* Start CA_WardsUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsUpdate];
GO
CREATE PROCEDURE [dbo].[CA_WardsUpdate]
	@Id int = null,
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	UPDATE [CA_Wards] SET
		[DistrictId] = @DistrictId,
		[Name] = @Name,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted
	WHERE [Id] = @Id
END
GO
/* End CA_WardsUpdate */

/* Start CA_WardsDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsDelete];
GO
CREATE PROCEDURE [dbo].[CA_WardsDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Wards]
	WHERE [Id] = @Id
END
GO
/* End CA_WardsDelete */

/* Start CA_WardsDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_WardsDeleteAll]
AS
BEGIN
	DELETE [CA_Wards]
END
GO
/* End CA_WardsDeleteAll */

/* Start CA_WardsCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_WardsCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_WardsCount];
GO
CREATE PROCEDURE [dbo].[CA_WardsCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Wards]
END
GO
/* End CA_WardsCount */
