
/* Start SY_BusinessGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessGetByID];
GO
CREATE PROCEDURE [dbo].[SY_BusinessGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[Id],
		[IsActived],
		[IsDeleted],
		[ProvinceId],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Business]
	WHERE [Id] = @Id
END
GO
/* End SY_BusinessGetByID */

/* Start SY_BusinessGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessGetAll];
GO
CREATE PROCEDURE [dbo].[SY_BusinessGetAll]
AS
BEGIN
	SELECT
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[Id],
		[IsActived],
		[IsDeleted],
		[ProvinceId],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Business]
END
GO
/* End SY_BusinessGetAll */

/* Start SY_BusinessGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_BusinessGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[Id],
		[IsActived],
		[IsDeleted],
		[ProvinceId],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Business]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_BusinessGetAllOnPage */

/* Start SY_BusinessInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessInsert];
GO
CREATE PROCEDURE [dbo].[SY_BusinessInsert]
	@WardsId int = null,
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@ProvinceId int = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Representative nvarchar(256) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	INSERT INTO [SY_Business]
	(
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[ProvinceId],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	)
	VALUES
	(
		@WardsId,
		@DistrictId,
		@Name,
		@Code,
		@IsActived,
		@IsDeleted,
		@ProvinceId,
		@Address,
		@Email,
		@Phone,
		@Representative,
		@IDCard,
		@Place,
		@NativePlace,
		@PermanentPlace,
		@Nation,
		@BirthDay,
		@Gender
	)
END
GO
/* End SY_BusinessGetByID */

/* Start SY_BusinessUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessUpdate];
GO
CREATE PROCEDURE [dbo].[SY_BusinessUpdate]
	@WardsId int = null,
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@Id bigint = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@ProvinceId int = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Representative nvarchar(256) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	UPDATE [SY_Business] SET
		[WardsId] = @WardsId,
		[DistrictId] = @DistrictId,
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[ProvinceId] = @ProvinceId,
		[Address] = @Address,
		[Email] = @Email,
		[Phone] = @Phone,
		[Representative] = @Representative,
		[IDCard] = @IDCard,
		[Place] = @Place,
		[NativePlace] = @NativePlace,
		[PermanentPlace] = @PermanentPlace,
		[Nation] = @Nation,
		[BirthDay] = @BirthDay,
		[Gender] = @Gender
	WHERE [Id] = @Id
END
GO
/* End SY_BusinessUpdate */

/* Start SY_BusinessDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessDelete];
GO
CREATE PROCEDURE [dbo].[SY_BusinessDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_Business]
	WHERE [Id] = @Id
END
GO
/* End SY_BusinessDelete */

/* Start SY_BusinessDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_BusinessDeleteAll]
AS
BEGIN
	DELETE [SY_Business]
END
GO
/* End SY_BusinessDeleteAll */

/* Start SY_BusinessCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_BusinessCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_BusinessCount];
GO
CREATE PROCEDURE [dbo].[SY_BusinessCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Business]
END
GO
/* End SY_BusinessCount */
