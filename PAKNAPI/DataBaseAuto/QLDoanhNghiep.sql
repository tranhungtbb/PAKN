
/* Start QL_DoanhNghiepGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepGetByID];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Phone],
		[Gender],
		[DOB],
		[Business],
		[Tax],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[Id],
		[RepresentativeName],
		[Email],
		[OrgProvince],
		[OrgDistrict],
		[OrgVillage],
		[OrgAddress],
		[OrgPhone],
		[OrgEmail],
		[RegistrationNum],
		[DecisionFoundation],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address]
	FROM [QL_DoanhNghiep]
	WHERE [Id] = @Id
END
GO
/* End QL_DoanhNghiepGetByID */

/* Start QL_DoanhNghiepGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepGetAll];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepGetAll]
AS
BEGIN
	SELECT
		[Phone],
		[Gender],
		[DOB],
		[Business],
		[Tax],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[Id],
		[RepresentativeName],
		[Email],
		[OrgProvince],
		[OrgDistrict],
		[OrgVillage],
		[OrgAddress],
		[OrgPhone],
		[OrgEmail],
		[RegistrationNum],
		[DecisionFoundation],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address]
	FROM [QL_DoanhNghiep]
END
GO
/* End QL_DoanhNghiepGetAll */

/* Start QL_DoanhNghiepGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Phone],
		[Gender],
		[DOB],
		[Business],
		[Tax],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[Id],
		[RepresentativeName],
		[Email],
		[OrgProvince],
		[OrgDistrict],
		[OrgVillage],
		[OrgAddress],
		[OrgPhone],
		[OrgEmail],
		[RegistrationNum],
		[DecisionFoundation],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address]
	FROM [QL_DoanhNghiep]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End QL_DoanhNghiepGetAllOnPage */

/* Start QL_DoanhNghiepInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepInsert];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepInsert]
	@Phone char(10) = null,
	@Gender bit = null,
	@DOB date = null,
	@Business nvarchar(255) = null,
	@Tax nvarchar(50) = null,
	@Status tinyint = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@RepresentativeName nvarchar(50) = null,
	@Email nvarchar(100) = null,
	@OrgProvince int = null,
	@OrgDistrict int = null,
	@OrgVillage int = null,
	@OrgAddress nvarchar(500) = null,
	@OrgPhone char(10) = null,
	@OrgEmail nvarchar(100) = null,
	@RegistrationNum nvarchar(50) = null,
	@DecisionFoundation nvarchar(100) = null,
	@DateIssue date = null,
	@Nation int = null,
	@Province int = null,
	@District int = null,
	@Village int = null,
	@Address nvarchar(500) = null
AS
BEGIN
	INSERT INTO [QL_DoanhNghiep]
	(
		[Phone],
		[Gender],
		[DOB],
		[Business],
		[Tax],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[RepresentativeName],
		[Email],
		[OrgProvince],
		[OrgDistrict],
		[OrgVillage],
		[OrgAddress],
		[OrgPhone],
		[OrgEmail],
		[RegistrationNum],
		[DecisionFoundation],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address]
	)
	VALUES
	(
		@Phone,
		@Gender,
		@DOB,
		@Business,
		@Tax,
		@Status,
		@CreatedBy,
		@CreatedDate,
		@UpdatedBy,
		@UpdatedDate,
		@RepresentativeName,
		@Email,
		@OrgProvince,
		@OrgDistrict,
		@OrgVillage,
		@OrgAddress,
		@OrgPhone,
		@OrgEmail,
		@RegistrationNum,
		@DecisionFoundation,
		@DateIssue,
		@Nation,
		@Province,
		@District,
		@Village,
		@Address
	)
END
GO
/* End QL_DoanhNghiepGetByID */

/* Start QL_DoanhNghiepUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepUpdate];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepUpdate]
	@Phone char(10) = null,
	@Gender bit = null,
	@DOB date = null,
	@Business nvarchar(255) = null,
	@Tax nvarchar(50) = null,
	@Status tinyint = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@Id int = null,
	@RepresentativeName nvarchar(50) = null,
	@Email nvarchar(100) = null,
	@OrgProvince int = null,
	@OrgDistrict int = null,
	@OrgVillage int = null,
	@OrgAddress nvarchar(500) = null,
	@OrgPhone char(10) = null,
	@OrgEmail nvarchar(100) = null,
	@RegistrationNum nvarchar(50) = null,
	@DecisionFoundation nvarchar(100) = null,
	@DateIssue date = null,
	@Nation int = null,
	@Province int = null,
	@District int = null,
	@Village int = null,
	@Address nvarchar(500) = null
AS
BEGIN
	UPDATE [QL_DoanhNghiep] SET
		[Phone] = @Phone,
		[Gender] = @Gender,
		[DOB] = @DOB,
		[Business] = @Business,
		[Tax] = @Tax,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedDate] = @UpdatedDate,
		[RepresentativeName] = @RepresentativeName,
		[Email] = @Email,
		[OrgProvince] = @OrgProvince,
		[OrgDistrict] = @OrgDistrict,
		[OrgVillage] = @OrgVillage,
		[OrgAddress] = @OrgAddress,
		[OrgPhone] = @OrgPhone,
		[OrgEmail] = @OrgEmail,
		[RegistrationNum] = @RegistrationNum,
		[DecisionFoundation] = @DecisionFoundation,
		[DateIssue] = @DateIssue,
		[Nation] = @Nation,
		[Province] = @Province,
		[District] = @District,
		[Village] = @Village,
		[Address] = @Address
	WHERE [Id] = @Id
END
GO
/* End QL_DoanhNghiepUpdate */

/* Start QL_DoanhNghiepDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepDelete];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepDelete]
	@Id int = null
AS
BEGIN
	DELETE [QL_DoanhNghiep]
	WHERE [Id] = @Id
END
GO
/* End QL_DoanhNghiepDelete */

/* Start QL_DoanhNghiepDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_DoanhNghiepDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_DoanhNghiepDeleteAll];
GO
CREATE PROCEDURE [dbo].[QL_DoanhNghiepDeleteAll]
AS
BEGIN
	DELETE [QL_DoanhNghiep]
END
GO
/* End QL_DoanhNghiepDeleteAll */
