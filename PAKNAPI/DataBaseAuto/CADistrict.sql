
/* Start CA_DistrictGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictGetByID];
GO
CREATE PROCEDURE [dbo].[CA_DistrictGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[ProvinceId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_District]
	WHERE [Id] = @Id
END
GO
/* End CA_DistrictGetByID */

/* Start CA_DistrictGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictGetAll];
GO
CREATE PROCEDURE [dbo].[CA_DistrictGetAll]
AS
BEGIN
	SELECT
		[Id],
		[ProvinceId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_District]
END
GO
/* End CA_DistrictGetAll */

/* Start CA_DistrictGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_DistrictGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[ProvinceId],
		[Name],
		[IsActived],
		[IsDeleted]
	FROM [CA_District]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_DistrictGetAllOnPage */

/* Start CA_DistrictInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictInsert];
GO
CREATE PROCEDURE [dbo].[CA_DistrictInsert]
	@ProvinceId int = null,
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	INSERT INTO [CA_District]
	(
		[ProvinceId],
		[Name],
		[IsActived],
		[IsDeleted]
	)
	VALUES
	(
		@ProvinceId,
		@Name,
		@IsActived,
		@IsDeleted
	)
END
GO
/* End CA_DistrictGetByID */

/* Start CA_DistrictUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictUpdate];
GO
CREATE PROCEDURE [dbo].[CA_DistrictUpdate]
	@Id int = null,
	@ProvinceId int = null,
	@Name nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null
AS
BEGIN
	UPDATE [CA_District] SET
		[ProvinceId] = @ProvinceId,
		[Name] = @Name,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted
	WHERE [Id] = @Id
END
GO
/* End CA_DistrictUpdate */

/* Start CA_DistrictDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictDelete];
GO
CREATE PROCEDURE [dbo].[CA_DistrictDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_District]
	WHERE [Id] = @Id
END
GO
/* End CA_DistrictDelete */

/* Start CA_DistrictDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_DistrictDeleteAll]
AS
BEGIN
	DELETE [CA_District]
END
GO
/* End CA_DistrictDeleteAll */

/* Start CA_DistrictCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_DistrictCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_DistrictCount];
GO
CREATE PROCEDURE [dbo].[CA_DistrictCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_District]
END
GO
/* End CA_DistrictCount */
