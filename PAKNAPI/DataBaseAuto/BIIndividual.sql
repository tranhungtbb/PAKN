
/* Start BI_IndividualGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualGetByID];
GO
CREATE PROCEDURE [dbo].[BI_IndividualGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[FullName],
		[IsActived],
		[IsDeleted],
		[Id],
		[UserId],
		[ProvinceId],
		[WardsId],
		[DistrictId],
		[DateOfIssue],
		[CreatedDate],
		[UpdatedDate],
		[CreatedBy],
		[UpdatedBy],
		[Status],
		[Code],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[IssuedPlace],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [BI_Individual]
	WHERE [Id] = @Id
END
GO
/* End BI_IndividualGetByID */

/* Start BI_IndividualGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualGetAll];
GO
CREATE PROCEDURE [dbo].[BI_IndividualGetAll]
AS
BEGIN
	SELECT
		[FullName],
		[IsActived],
		[IsDeleted],
		[Id],
		[UserId],
		[ProvinceId],
		[WardsId],
		[DistrictId],
		[DateOfIssue],
		[CreatedDate],
		[UpdatedDate],
		[CreatedBy],
		[UpdatedBy],
		[Status],
		[Code],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[IssuedPlace],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [BI_Individual]
END
GO
/* End BI_IndividualGetAll */

/* Start BI_IndividualGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[BI_IndividualGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[FullName],
		[IsActived],
		[IsDeleted],
		[Id],
		[UserId],
		[ProvinceId],
		[WardsId],
		[DistrictId],
		[DateOfIssue],
		[CreatedDate],
		[UpdatedDate],
		[CreatedBy],
		[UpdatedBy],
		[Status],
		[Code],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[IssuedPlace],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	FROM [BI_Individual]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End BI_IndividualGetAllOnPage */

/* Start BI_IndividualInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualInsert];
GO
CREATE PROCEDURE [dbo].[BI_IndividualInsert]
	@FullName nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@UserId bigint = null,
	@ProvinceId int = null,
	@WardsId int = null,
	@DistrictId int = null,
	@DateOfIssue date = null,
	@CreatedDate datetime = null,
	@UpdatedDate datetime = null,
	@CreatedBy int = null,
	@UpdatedBy int = null,
	@Status int = null,
	@Code nvarchar(256) = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@IDCard varchar(15) = null,
	@IssuedPlace nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	INSERT INTO [BI_Individual]
	(
		[FullName],
		[IsActived],
		[IsDeleted],
		[UserId],
		[ProvinceId],
		[WardsId],
		[DistrictId],
		[DateOfIssue],
		[CreatedDate],
		[UpdatedDate],
		[CreatedBy],
		[UpdatedBy],
		[Status],
		[Code],
		[Address],
		[Email],
		[Phone],
		[IDCard],
		[IssuedPlace],
		[NativePlace],
		[PermanentPlace],
		[Nation],
		[BirthDay],
		[Gender]
	)
	VALUES
	(
		@FullName,
		@IsActived,
		@IsDeleted,
		@UserId,
		@ProvinceId,
		@WardsId,
		@DistrictId,
		@DateOfIssue,
		@CreatedDate,
		@UpdatedDate,
		@CreatedBy,
		@UpdatedBy,
		@Status,
		@Code,
		@Address,
		@Email,
		@Phone,
		@IDCard,
		@IssuedPlace,
		@NativePlace,
		@PermanentPlace,
		@Nation,
		@BirthDay,
		@Gender
	)
END
GO
/* End BI_IndividualGetByID */

/* Start BI_IndividualUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualUpdate];
GO
CREATE PROCEDURE [dbo].[BI_IndividualUpdate]
	@FullName nvarchar(256) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Id bigint = null,
	@UserId bigint = null,
	@ProvinceId int = null,
	@WardsId int = null,
	@DistrictId int = null,
	@DateOfIssue date = null,
	@CreatedDate datetime = null,
	@UpdatedDate datetime = null,
	@CreatedBy int = null,
	@UpdatedBy int = null,
	@Status int = null,
	@Code nvarchar(256) = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@IDCard varchar(15) = null,
	@IssuedPlace nvarchar(256) = null,
	@NativePlace nvarchar(256) = null,
	@PermanentPlace nvarchar(256) = null,
	@Nation nvarchar(256) = null,
	@BirthDay datetime = null,
	@Gender bit = null
AS
BEGIN
	UPDATE [BI_Individual] SET
		[FullName] = @FullName,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[UserId] = @UserId,
		[ProvinceId] = @ProvinceId,
		[WardsId] = @WardsId,
		[DistrictId] = @DistrictId,
		[DateOfIssue] = @DateOfIssue,
		[CreatedDate] = @CreatedDate,
		[UpdatedDate] = @UpdatedDate,
		[CreatedBy] = @CreatedBy,
		[UpdatedBy] = @UpdatedBy,
		[Status] = @Status,
		[Code] = @Code,
		[Address] = @Address,
		[Email] = @Email,
		[Phone] = @Phone,
		[IDCard] = @IDCard,
		[IssuedPlace] = @IssuedPlace,
		[NativePlace] = @NativePlace,
		[PermanentPlace] = @PermanentPlace,
		[Nation] = @Nation,
		[BirthDay] = @BirthDay,
		[Gender] = @Gender
	WHERE [Id] = @Id
END
GO
/* End BI_IndividualUpdate */

/* Start BI_IndividualDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualDelete];
GO
CREATE PROCEDURE [dbo].[BI_IndividualDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [BI_Individual]
	WHERE [Id] = @Id
END
GO
/* End BI_IndividualDelete */

/* Start BI_IndividualDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualDeleteAll];
GO
CREATE PROCEDURE [dbo].[BI_IndividualDeleteAll]
AS
BEGIN
	DELETE [BI_Individual]
END
GO
/* End BI_IndividualDeleteAll */

/* Start BI_IndividualCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[BI_IndividualCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [BI_IndividualCount];
GO
CREATE PROCEDURE [dbo].[BI_IndividualCount]
AS
BEGIN
	SELECT Count(*)
	FROM [BI_Individual]
END
GO
/* End BI_IndividualCount */
