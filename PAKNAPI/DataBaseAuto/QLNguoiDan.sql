
/* Start QL_NguoiDanGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanGetByID];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Phone],
		[Identity],
		[Gender],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[DOB],
		[FullName],
		[Id],
		[PlaceIssue],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address],
		[Email]
	FROM [QL_NguoiDan]
	WHERE [Id] = @Id
END
GO
/* End QL_NguoiDanGetByID */

/* Start QL_NguoiDanGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanGetAll];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanGetAll]
AS
BEGIN
	SELECT
		[Phone],
		[Identity],
		[Gender],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[DOB],
		[FullName],
		[Id],
		[PlaceIssue],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address],
		[Email]
	FROM [QL_NguoiDan]
END
GO
/* End QL_NguoiDanGetAll */

/* Start QL_NguoiDanGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Phone],
		[Identity],
		[Gender],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[DOB],
		[FullName],
		[Id],
		[PlaceIssue],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address],
		[Email]
	FROM [QL_NguoiDan]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End QL_NguoiDanGetAllOnPage */

/* Start QL_NguoiDanInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanInsert];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanInsert]
	@Phone char(10) = null,
	@Identity int = null,
	@Gender bit = null,
	@Status tinyint = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@DOB date = null,
	@FullName nvarchar(50) = null,
	@PlaceIssue nvarchar(500) = null,
	@DateIssue date = null,
	@Nation int = null,
	@Province int = null,
	@District int = null,
	@Village int = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(100) = null
AS
BEGIN
	INSERT INTO [QL_NguoiDan]
	(
		[Phone],
		[Identity],
		[Gender],
		[Status],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[DOB],
		[FullName],
		[PlaceIssue],
		[DateIssue],
		[Nation],
		[Province],
		[District],
		[Village],
		[Address],
		[Email]
	)
	VALUES
	(
		@Phone,
		@Identity,
		@Gender,
		@Status,
		@CreatedBy,
		@CreatedDate,
		@UpdatedBy,
		@UpdatedDate,
		@DOB,
		@FullName,
		@PlaceIssue,
		@DateIssue,
		@Nation,
		@Province,
		@District,
		@Village,
		@Address,
		@Email
	)
END
GO
/* End QL_NguoiDanGetByID */

/* Start QL_NguoiDanUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanUpdate];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanUpdate]
	@Phone char(10) = null,
	@Identity int = null,
	@Gender bit = null,
	@Status tinyint = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@DOB date = null,
	@FullName nvarchar(50) = null,
	@Id int = null,
	@PlaceIssue nvarchar(500) = null,
	@DateIssue date = null,
	@Nation int = null,
	@Province int = null,
	@District int = null,
	@Village int = null,
	@Address nvarchar(500) = null,
	@Email nvarchar(100) = null
AS
BEGIN
	UPDATE [QL_NguoiDan] SET
		[Phone] = @Phone,
		[Identity] = @Identity,
		[Gender] = @Gender,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedDate] = @UpdatedDate,
		[DOB] = @DOB,
		[FullName] = @FullName,
		[PlaceIssue] = @PlaceIssue,
		[DateIssue] = @DateIssue,
		[Nation] = @Nation,
		[Province] = @Province,
		[District] = @District,
		[Village] = @Village,
		[Address] = @Address,
		[Email] = @Email
	WHERE [Id] = @Id
END
GO
/* End QL_NguoiDanUpdate */

/* Start QL_NguoiDanDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanDelete];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanDelete]
	@Id int = null
AS
BEGIN
	DELETE [QL_NguoiDan]
	WHERE [Id] = @Id
END
GO
/* End QL_NguoiDanDelete */

/* Start QL_NguoiDanDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[QL_NguoiDanDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [QL_NguoiDanDeleteAll];
GO
CREATE PROCEDURE [dbo].[QL_NguoiDanDeleteAll]
AS
BEGIN
	DELETE [QL_NguoiDan]
END
GO
/* End QL_NguoiDanDeleteAll */
