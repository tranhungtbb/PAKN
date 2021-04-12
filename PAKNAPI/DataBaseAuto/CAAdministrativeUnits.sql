
/* Start CA_AdministrativeUnitsGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsGetByID];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsGetByID]
	@Id smallint = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[Description],
		[ProvinceId],
		[DistrictId],
		[Level],
		[Status],
		[IsDeleted],
		[IsACtive]
	FROM [CA_AdministrativeUnits]
	WHERE [Id] = @Id
END
GO
/* End CA_AdministrativeUnitsGetByID */

/* Start CA_AdministrativeUnitsGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsGetAll];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[Description],
		[ProvinceId],
		[DistrictId],
		[Level],
		[Status],
		[IsDeleted],
		[IsACtive]
	FROM [CA_AdministrativeUnits]
END
GO
/* End CA_AdministrativeUnitsGetAll */

/* Start CA_AdministrativeUnitsGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[Description],
		[ProvinceId],
		[DistrictId],
		[Level],
		[Status],
		[IsDeleted],
		[IsACtive]
	FROM [CA_AdministrativeUnits]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_AdministrativeUnitsGetAllOnPage */

/* Start CA_AdministrativeUnitsInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsInsert];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsInsert]
	@Name nvarchar(100) = null,
	@Code varchar(20) = null,
	@Description nvarchar(255) = null,
	@ProvinceId smallint = null,
	@DistrictId smallint = null,
	@Level tinyint = null,
	@Status bit = null,
	@IsDeleted bit = null,
	@IsACtive bit = null
AS
BEGIN
	INSERT INTO [CA_AdministrativeUnits]
	(
		[Name],
		[Code],
		[Description],
		[ProvinceId],
		[DistrictId],
		[Level],
		[Status],
		[IsDeleted],
		[IsACtive]
	)
	VALUES
	(
		@Name,
		@Code,
		@Description,
		@ProvinceId,
		@DistrictId,
		@Level,
		@Status,
		@IsDeleted,
		@IsACtive
	)
END
GO
/* End CA_AdministrativeUnitsGetByID */

/* Start CA_AdministrativeUnitsUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsUpdate];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsUpdate]
	@Id smallint = null,
	@Name nvarchar(100) = null,
	@Code varchar(20) = null,
	@Description nvarchar(255) = null,
	@ProvinceId smallint = null,
	@DistrictId smallint = null,
	@Level tinyint = null,
	@Status bit = null,
	@IsDeleted bit = null,
	@IsACtive bit = null
AS
BEGIN
	UPDATE [CA_AdministrativeUnits] SET
		[Name] = @Name,
		[Code] = @Code,
		[Description] = @Description,
		[ProvinceId] = @ProvinceId,
		[DistrictId] = @DistrictId,
		[Level] = @Level,
		[Status] = @Status,
		[IsDeleted] = @IsDeleted,
		[IsACtive] = @IsACtive
	WHERE [Id] = @Id
END
GO
/* End CA_AdministrativeUnitsUpdate */

/* Start CA_AdministrativeUnitsDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsDelete];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsDelete]
	@Id smallint = null
AS
BEGIN
	DELETE [CA_AdministrativeUnits]
	WHERE [Id] = @Id
END
GO
/* End CA_AdministrativeUnitsDelete */

/* Start CA_AdministrativeUnitsDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_AdministrativeUnitsDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_AdministrativeUnitsDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_AdministrativeUnitsDeleteAll]
AS
BEGIN
	DELETE [CA_AdministrativeUnits]
END
GO
/* End CA_AdministrativeUnitsDeleteAll */
