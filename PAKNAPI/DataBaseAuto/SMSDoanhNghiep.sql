
/* Start SMS_DoanhNghiepGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepGetByID];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[BusinessId]
	FROM [SMS_DoanhNghiep]
	WHERE [Id] = @Id
END
GO
/* End SMS_DoanhNghiepGetByID */

/* Start SMS_DoanhNghiepGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepGetAll];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepGetAll]
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[BusinessId]
	FROM [SMS_DoanhNghiep]
END
GO
/* End SMS_DoanhNghiepGetAll */

/* Start SMS_DoanhNghiepGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[SMSId],
		[BusinessId]
	FROM [SMS_DoanhNghiep]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SMS_DoanhNghiepGetAllOnPage */

/* Start SMS_DoanhNghiepInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepInsert];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepInsert]
	@SMSId int = null,
	@BusinessId int = null
AS
BEGIN
	INSERT INTO [SMS_DoanhNghiep]
	(
		[SMSId],
		[BusinessId]
	)
	VALUES
	(
		@SMSId,
		@BusinessId
	)
END
GO
/* End SMS_DoanhNghiepGetByID */

/* Start SMS_DoanhNghiepUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepUpdate];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepUpdate]
	@Id int = null,
	@SMSId int = null,
	@BusinessId int = null
AS
BEGIN
	UPDATE [SMS_DoanhNghiep] SET
		[SMSId] = @SMSId,
		[BusinessId] = @BusinessId
	WHERE [Id] = @Id
END
GO
/* End SMS_DoanhNghiepUpdate */

/* Start SMS_DoanhNghiepDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepDelete];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepDelete]
	@Id int = null
AS
BEGIN
	DELETE [SMS_DoanhNghiep]
	WHERE [Id] = @Id
END
GO
/* End SMS_DoanhNghiepDelete */

/* Start SMS_DoanhNghiepDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_DoanhNghiepDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_DoanhNghiepDeleteAll];
GO
CREATE PROCEDURE [dbo].[SMS_DoanhNghiepDeleteAll]
AS
BEGIN
	DELETE [SMS_DoanhNghiep]
END
GO
/* End SMS_DoanhNghiepDeleteAll */
