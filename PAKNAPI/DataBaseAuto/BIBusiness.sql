
/* Start BI_BusinessGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessGetByID];
GO
CREATE PROCEDURE [dbo].[BI_BusinessGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Id],
		[ProvinceId],
		[BusinessRegistration],
		[DecisionOfEstablishing],
		[DateOfIssue],
		[Tax],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation]
	FROM [BI_Business]
	WHERE [Id] = @Id
END
GO
/* End BI_BusinessGetByID */

/* Start BI_BusinessGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessGetAll];
GO
CREATE PROCEDURE [dbo].[BI_BusinessGetAll]
AS
BEGIN
	SELECT
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[Id],
		[ProvinceId],
		[BusinessRegistration],
		[DecisionOfEstablishing],
		[DateOfIssue],
		[Tax],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation]
	FROM [BI_Business]
END
GO
/* End BI_BusinessGetAll */

/* Start BI_BusinessGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[BI_BusinessGetAllOnPage]
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
		[IsActived],
		[IsDeleted],
		[Id],
		[ProvinceId],
		[BusinessRegistration],
		[DecisionOfEstablishing],
		[DateOfIssue],
		[Tax],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation]
	FROM [BI_Business]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End BI_BusinessGetAllOnPage */

/* Start BI_BusinessInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessInsert];
GO
CREATE PROCEDURE [dbo].[BI_BusinessInsert]
	@WardsId int = null,
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@ProvinceId int = null,
	@BusinessRegistration nvarchar(50) = null,
	@DecisionOfEstablishing nvarchar(100) = null,
	@DateOfIssue date = null,
	@Tax nvarchar(50) = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Representative nvarchar(256) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null
AS
BEGIN
	INSERT INTO [BI_Business]
	(
		[WardsId],
		[DistrictId],
		[Name],
		[Code],
		[IsActived],
		[IsDeleted],
		[ProvinceId],
		[BusinessRegistration],
		[DecisionOfEstablishing],
		[DateOfIssue],
		[Tax],
		[Address],
		[Email],
		[Phone],
		[Representative],
		[IDCard],
		[Place],
		[NativePlace],
		[PermanentPlace],
		[Nation]
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
		@BusinessRegistration,
		@DecisionOfEstablishing,
		@DateOfIssue,
		@Tax,
		@Address,
		@Email,
		@Phone,
		@Representative,
		@IDCard,
		@Place,
		@NativePlace,
		@PermanentPlace,
		@Nation
	)
END
GO
/* End BI_BusinessGetByID */

/* Start BI_BusinessUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessUpdate];
GO
CREATE PROCEDURE [dbo].[BI_BusinessUpdate]
	@WardsId int = null,
	@DistrictId int = null,
	@Name nvarchar(256) = null,
	@Code nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Id bigint = null,
	@ProvinceId int = null,
	@BusinessRegistration nvarchar(50) = null,
	@DecisionOfEstablishing nvarchar(100) = null,
	@DateOfIssue date = null,
	@Tax nvarchar(50) = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@Representative nvarchar(256) = null,
	@IDCard varchar(15) = null,
	@Place nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null
AS
BEGIN
	UPDATE [BI_Business] SET
		[WardsId] = @WardsId,
		[DistrictId] = @DistrictId,
		[Name] = @Name,
		[Code] = @Code,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[ProvinceId] = @ProvinceId,
		[BusinessRegistration] = @BusinessRegistration,
		[DecisionOfEstablishing] = @DecisionOfEstablishing,
		[DateOfIssue] = @DateOfIssue,
		[Tax] = @Tax,
		[Address] = @Address,
		[Email] = @Email,
		[Phone] = @Phone,
		[Representative] = @Representative,
		[IDCard] = @IDCard,
		[Place] = @Place,
		[NativePlace] = @NativePlace,
		[PermanentPlace] = @PermanentPlace,
		[Nation] = @Nation
	WHERE [Id] = @Id
END
GO
/* End BI_BusinessUpdate */

/* Start BI_BusinessDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessDelete];
GO
CREATE PROCEDURE [dbo].[BI_BusinessDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [BI_Business]
	WHERE [Id] = @Id
END
GO
/* End BI_BusinessDelete */

/* Start BI_BusinessDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessDeleteAll];
GO
CREATE PROCEDURE [dbo].[BI_BusinessDeleteAll]
AS
BEGIN
	DELETE [BI_Business]
END
GO
/* End BI_BusinessDeleteAll */

/* Start BI_BusinessCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_BusinessCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_BusinessCount];
GO
CREATE PROCEDURE [dbo].[BI_BusinessCount]
AS
BEGIN
	SELECT Count(*)
	FROM [BI_Business]
END
GO
/* End BI_BusinessCount */
