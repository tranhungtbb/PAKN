
/* Start SMS_NguoiNhanGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanGetByID];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[IndividualId]
	FROM [SMS_NguoiNhan]
	WHERE [Id] = @Id
END
GO
/* End SMS_NguoiNhanGetByID */

/* Start SMS_NguoiNhanGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanGetAll];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanGetAll]
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[IndividualId]
	FROM [SMS_NguoiNhan]
END
GO
/* End SMS_NguoiNhanGetAll */

/* Start SMS_NguoiNhanGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[SMSId],
		[IndividualId]
	FROM [SMS_NguoiNhan]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SMS_NguoiNhanGetAllOnPage */

/* Start SMS_NguoiNhanInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanInsert];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanInsert]
	@SMSId int = null,
	@IndividualId int = null
AS
BEGIN
	INSERT INTO [SMS_NguoiNhan]
	(
		[SMSId],
		[IndividualId]
	)
	VALUES
	(
		@SMSId,
		@IndividualId
	)
END
GO
/* End SMS_NguoiNhanGetByID */

/* Start SMS_NguoiNhanUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanUpdate];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanUpdate]
	@Id int = null,
	@SMSId int = null,
	@IndividualId int = null
AS
BEGIN
	UPDATE [SMS_NguoiNhan] SET
		[SMSId] = @SMSId,
		[IndividualId] = @IndividualId
	WHERE [Id] = @Id
END
GO
/* End SMS_NguoiNhanUpdate */

/* Start SMS_NguoiNhanDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanDelete];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanDelete]
	@Id int = null
AS
BEGIN
	DELETE [SMS_NguoiNhan]
	WHERE [Id] = @Id
END
GO
/* End SMS_NguoiNhanDelete */

/* Start SMS_NguoiNhanDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_NguoiNhanDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_NguoiNhanDeleteAll];
GO
CREATE PROCEDURE [dbo].[SMS_NguoiNhanDeleteAll]
AS
BEGIN
	DELETE [SMS_NguoiNhan]
END
GO
/* End SMS_NguoiNhanDeleteAll */
