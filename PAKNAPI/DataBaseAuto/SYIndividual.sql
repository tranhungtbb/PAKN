
/* Start SY_IndividualGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualGetByID];
GO
CREATE PROCEDURE [dbo].[SY_IndividualGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[WardsId],
		[DistrictId],
		[FullName],
		[Code],
		[IsActived],
		[IsDeleted],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Individual]
	WHERE [Id] = @Id
END
GO
/* End SY_IndividualGetByID */

/* Start SY_IndividualGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualGetAll];
GO
CREATE PROCEDURE [dbo].[SY_IndividualGetAll]
AS
BEGIN
	SELECT
		[Id],
		[WardsId],
		[DistrictId],
		[FullName],
		[Code],
		[IsActived],
		[IsDeleted],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Individual]
END
GO
/* End SY_IndividualGetAll */

/* Start SY_IndividualGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_IndividualGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[WardsId],
		[DistrictId],
		[FullName],
		[Code],
		[IsActived],
		[IsDeleted],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [SY_Individual]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_IndividualGetAllOnPage */

/* Start SY_IndividualInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualInsert];
GO
CREATE PROCEDURE [dbo].[SY_IndividualInsert]
	@WardsId int = null,
	@DistrictId int = null,
	@FullName nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	INSERT INTO [SY_Individual]
	(
		[WardsId],
		[DistrictId],
		[FullName],
		[Code],
		[IsActived],
		[IsDeleted],
		[Address],
		[Email],
		[Phone],
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
		@FullName,
		@Code,
		@IsActived,
		@IsDeleted,
		@Address,
		@Email,
		@Phone,
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
/* End SY_IndividualGetByID */

/* Start SY_IndividualUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualUpdate];
GO
CREATE PROCEDURE [dbo].[SY_IndividualUpdate]
	@Id bigint = null,
	@WardsId int = null,
	@DistrictId int = null,
	@FullName nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	UPDATE [SY_Individual] SET
		[WardsId] = @WardsId,
		[DistrictId] = @DistrictId,
		[FullName] = @FullName,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Address] = @Address,
		[Email] = @Email,
		[Phone] = @Phone,
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
/* End SY_IndividualUpdate */

/* Start SY_IndividualDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualDelete];
GO
CREATE PROCEDURE [dbo].[SY_IndividualDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_Individual]
	WHERE [Id] = @Id
END
GO
/* End SY_IndividualDelete */

/* Start SY_IndividualDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_IndividualDeleteAll]
AS
BEGIN
	DELETE [SY_Individual]
END
GO
/* End SY_IndividualDeleteAll */

/* Start SY_IndividualCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_IndividualCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_IndividualCount];
GO
CREATE PROCEDURE [dbo].[SY_IndividualCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Individual]
END
GO
/* End SY_IndividualCount */
