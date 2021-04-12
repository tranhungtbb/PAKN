
/* Start DAM_AdministrationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[CountryCode],
		[UnitReceive],
		[Field],
		[RankReceive],
		[TypeSend],
		[FileNum],
		[AmountTime],
		[Proceed],
		[Object],
		[Organization],
		[ImpactAssessment],
		[Note],
		[Status],
		[IsShow],
		[Result],
		[LegalGrounds],
		[Request],
		[OrganizationDecision],
		[Address],
		[OrganizationAuthor],
		[OrganizationCombine]
	FROM [DAM_Administration]
	WHERE [Id] = @Id
END
GO
/* End DAM_AdministrationGetByID */

/* Start DAM_AdministrationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Code],
		[CountryCode],
		[UnitReceive],
		[Field],
		[RankReceive],
		[TypeSend],
		[FileNum],
		[AmountTime],
		[Proceed],
		[Object],
		[Organization],
		[ImpactAssessment],
		[Note],
		[Status],
		[IsShow],
		[Result],
		[LegalGrounds],
		[Request],
		[OrganizationDecision],
		[Address],
		[OrganizationAuthor],
		[OrganizationCombine]
	FROM [DAM_Administration]
END
GO
/* End DAM_AdministrationGetAll */

/* Start DAM_AdministrationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Code],
		[CountryCode],
		[UnitReceive],
		[Field],
		[RankReceive],
		[TypeSend],
		[FileNum],
		[AmountTime],
		[Proceed],
		[Object],
		[Organization],
		[ImpactAssessment],
		[Note],
		[Status],
		[IsShow],
		[Result],
		[LegalGrounds],
		[Request],
		[OrganizationDecision],
		[Address],
		[OrganizationAuthor],
		[OrganizationCombine]
	FROM [DAM_Administration]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_AdministrationGetAllOnPage */

/* Start DAM_AdministrationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationInsert];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationInsert]
	@Name nvarchar(1000) = null,
	@Code nvarchar(100) = null,
	@CountryCode nvarchar(100) = null,
	@UnitReceive int = null,
	@Field int = null,
	@RankReceive int = null,
	@TypeSend bit = null,
	@FileNum nvarchar(255) = null,
	@AmountTime nvarchar(255) = null,
	@Proceed nvarchar(1000) = null,
	@Object nvarchar(1000) = null,
	@Organization nvarchar(500) = null,
	@ImpactAssessment nvarchar(1000) = null,
	@Note nvarchar(255) = null,
	@Status tinyint = null,
	@IsShow bit = null,
	@Result nvarchar(500) = null,
	@LegalGrounds nvarchar(2000) = null,
	@Request nvarchar(1000) = null,
	@OrganizationDecision nvarchar(500) = null,
	@Address nvarchar(500) = null,
	@OrganizationAuthor nvarchar(500) = null,
	@OrganizationCombine nvarchar(500) = null
AS
BEGIN
	INSERT INTO [DAM_Administration]
	(
		[Name],
		[Code],
		[CountryCode],
		[UnitReceive],
		[Field],
		[RankReceive],
		[TypeSend],
		[FileNum],
		[AmountTime],
		[Proceed],
		[Object],
		[Organization],
		[ImpactAssessment],
		[Note],
		[Status],
		[IsShow],
		[Result],
		[LegalGrounds],
		[Request],
		[OrganizationDecision],
		[Address],
		[OrganizationAuthor],
		[OrganizationCombine]
	)
	VALUES
	(
		@Name,
		@Code,
		@CountryCode,
		@UnitReceive,
		@Field,
		@RankReceive,
		@TypeSend,
		@FileNum,
		@AmountTime,
		@Proceed,
		@Object,
		@Organization,
		@ImpactAssessment,
		@Note,
		@Status,
		@IsShow,
		@Result,
		@LegalGrounds,
		@Request,
		@OrganizationDecision,
		@Address,
		@OrganizationAuthor,
		@OrganizationCombine
	)
END
GO
/* End DAM_AdministrationGetByID */

/* Start DAM_AdministrationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationUpdate]
	@Id int = null,
	@Name nvarchar(1000) = null,
	@Code nvarchar(100) = null,
	@CountryCode nvarchar(100) = null,
	@UnitReceive int = null,
	@Field int = null,
	@RankReceive int = null,
	@TypeSend bit = null,
	@FileNum nvarchar(255) = null,
	@AmountTime nvarchar(255) = null,
	@Proceed nvarchar(1000) = null,
	@Object nvarchar(1000) = null,
	@Organization nvarchar(500) = null,
	@ImpactAssessment nvarchar(1000) = null,
	@Note nvarchar(255) = null,
	@Status tinyint = null,
	@IsShow bit = null,
	@Result nvarchar(500) = null,
	@LegalGrounds nvarchar(2000) = null,
	@Request nvarchar(1000) = null,
	@OrganizationDecision nvarchar(500) = null,
	@Address nvarchar(500) = null,
	@OrganizationAuthor nvarchar(500) = null,
	@OrganizationCombine nvarchar(500) = null
AS
BEGIN
	UPDATE [DAM_Administration] SET
		[Name] = @Name,
		[Code] = @Code,
		[CountryCode] = @CountryCode,
		[UnitReceive] = @UnitReceive,
		[Field] = @Field,
		[RankReceive] = @RankReceive,
		[TypeSend] = @TypeSend,
		[FileNum] = @FileNum,
		[AmountTime] = @AmountTime,
		[Proceed] = @Proceed,
		[Object] = @Object,
		[Organization] = @Organization,
		[ImpactAssessment] = @ImpactAssessment,
		[Note] = @Note,
		[Status] = @Status,
		[IsShow] = @IsShow,
		[Result] = @Result,
		[LegalGrounds] = @LegalGrounds,
		[Request] = @Request,
		[OrganizationDecision] = @OrganizationDecision,
		[Address] = @Address,
		[OrganizationAuthor] = @OrganizationAuthor,
		[OrganizationCombine] = @OrganizationCombine
	WHERE [Id] = @Id
END
GO
/* End DAM_AdministrationUpdate */

/* Start DAM_AdministrationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationDelete];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_Administration]
	WHERE [Id] = @Id
END
GO
/* End DAM_AdministrationDelete */

/* Start DAM_AdministrationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_AdministrationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_AdministrationDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_AdministrationDeleteAll]
AS
BEGIN
	DELETE [DAM_Administration]
END
GO
/* End DAM_AdministrationDeleteAll */
