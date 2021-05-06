
/* Start SMS_QuanLyTinNhanGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanGetByID];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Title],
		[Content],
		[Signature],
		[CreateDate],
		[UserCreateId],
		[UnitId],
		[Status],
		[UpdateDate],
		[UserUpdateId],
		[SendDate],
		[UserSend]
	FROM [SMS_QuanLyTinNhan]
	WHERE [Id] = @Id
END
GO
/* End SMS_QuanLyTinNhanGetByID */

/* Start SMS_QuanLyTinNhanGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanGetAll];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Title],
		[Content],
		[Signature],
		[CreateDate],
		[UserCreateId],
		[UnitId],
		[Status],
		[UpdateDate],
		[UserUpdateId],
		[SendDate],
		[UserSend]
	FROM [SMS_QuanLyTinNhan]
END
GO
/* End SMS_QuanLyTinNhanGetAll */

/* Start SMS_QuanLyTinNhanGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Title],
		[Content],
		[Signature],
		[CreateDate],
		[UserCreateId],
		[UnitId],
		[Status],
		[UpdateDate],
		[UserUpdateId],
		[SendDate],
		[UserSend]
	FROM [SMS_QuanLyTinNhan]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SMS_QuanLyTinNhanGetAllOnPage */

/* Start SMS_QuanLyTinNhanInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanInsert];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanInsert]
	@Title nvarchar(50) = null,
	@Content nvarchar(100) = null,
	@Signature nvarchar(50) = null,
	@CreateDate datetime = null,
	@UserCreateId int = null,
	@UnitId int = null,
	@Status tinyint = null,
	@UpdateDate datetime = null,
	@UserUpdateId int = null,
	@SendDate datetime = null,
	@UserSend int = null
AS
BEGIN
	INSERT INTO [SMS_QuanLyTinNhan]
	(
		[Title],
		[Content],
		[Signature],
		[CreateDate],
		[UserCreateId],
		[UnitId],
		[Status],
		[UpdateDate],
		[UserUpdateId],
		[SendDate],
		[UserSend]
	)
	VALUES
	(
		@Title,
		@Content,
		@Signature,
		@CreateDate,
		@UserCreateId,
		@UnitId,
		@Status,
		@UpdateDate,
		@UserUpdateId,
		@SendDate,
		@UserSend
	)
END
GO
/* End SMS_QuanLyTinNhanGetByID */

/* Start SMS_QuanLyTinNhanUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanUpdate];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanUpdate]
	@Id int = null,
	@Title nvarchar(50) = null,
	@Content nvarchar(100) = null,
	@Signature nvarchar(50) = null,
	@CreateDate datetime = null,
	@UserCreateId int = null,
	@UnitId int = null,
	@Status tinyint = null,
	@UpdateDate datetime = null,
	@UserUpdateId int = null,
	@SendDate datetime = null,
	@UserSend int = null
AS
BEGIN
	UPDATE [SMS_QuanLyTinNhan] SET
		[Title] = @Title,
		[Content] = @Content,
		[Signature] = @Signature,
		[CreateDate] = @CreateDate,
		[UserCreateId] = @UserCreateId,
		[UnitId] = @UnitId,
		[Status] = @Status,
		[UpdateDate] = @UpdateDate,
		[UserUpdateId] = @UserUpdateId,
		[SendDate] = @SendDate,
		[UserSend] = @UserSend
	WHERE [Id] = @Id
END
GO
/* End SMS_QuanLyTinNhanUpdate */

/* Start SMS_QuanLyTinNhanDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanDelete];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanDelete]
	@Id int = null
AS
BEGIN
	DELETE [SMS_QuanLyTinNhan]
	WHERE [Id] = @Id
END
GO
/* End SMS_QuanLyTinNhanDelete */

/* Start SMS_QuanLyTinNhanDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_QuanLyTinNhanDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_QuanLyTinNhanDeleteAll];
GO
CREATE PROCEDURE [dbo].[SMS_QuanLyTinNhanDeleteAll]
AS
BEGIN
	DELETE [SMS_QuanLyTinNhan]
END
GO
/* End SMS_QuanLyTinNhanDeleteAll */
