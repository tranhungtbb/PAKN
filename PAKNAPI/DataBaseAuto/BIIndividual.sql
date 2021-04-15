
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
	 = null
AS
BEGIN
	SELECT
		[FullName],
		[IsActived],
		[IsDeleted],
		[Id],
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
	WHERE [] = @
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
