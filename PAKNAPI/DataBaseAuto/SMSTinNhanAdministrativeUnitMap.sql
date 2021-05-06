
/* Start SMS_TinNhan_AdministrativeUnit_MapGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapGetByID];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[AdministrativeUnitId]
	FROM [SMS_TinNhan_AdministrativeUnit_Map]
	WHERE [Id] = @Id
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapGetByID */

/* Start SMS_TinNhan_AdministrativeUnit_MapGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapGetAll];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapGetAll]
AS
BEGIN
	SELECT
		[Id],
		[SMSId],
		[AdministrativeUnitId]
	FROM [SMS_TinNhan_AdministrativeUnit_Map]
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapGetAll */

/* Start SMS_TinNhan_AdministrativeUnit_MapGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[SMSId],
		[AdministrativeUnitId]
	FROM [SMS_TinNhan_AdministrativeUnit_Map]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapGetAllOnPage */

/* Start SMS_TinNhan_AdministrativeUnit_MapInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapInsert];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapInsert]
	@SMSId int = null,
	@AdministrativeUnitId int = null
AS
BEGIN
	INSERT INTO [SMS_TinNhan_AdministrativeUnit_Map]
	(
		[SMSId],
		[AdministrativeUnitId]
	)
	VALUES
	(
		@SMSId,
		@AdministrativeUnitId
	)
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapGetByID */

/* Start SMS_TinNhan_AdministrativeUnit_MapUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapUpdate];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapUpdate]
	@Id int = null,
	@SMSId int = null,
	@AdministrativeUnitId int = null
AS
BEGIN
	UPDATE [SMS_TinNhan_AdministrativeUnit_Map] SET
		[SMSId] = @SMSId,
		[AdministrativeUnitId] = @AdministrativeUnitId
	WHERE [Id] = @Id
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapUpdate */

/* Start SMS_TinNhan_AdministrativeUnit_MapDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapDelete];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapDelete]
	@Id int = null
AS
BEGIN
	DELETE [SMS_TinNhan_AdministrativeUnit_Map]
	WHERE [Id] = @Id
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapDelete */

/* Start SMS_TinNhan_AdministrativeUnit_MapDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SMS_TinNhan_AdministrativeUnit_MapDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SMS_TinNhan_AdministrativeUnit_MapDeleteAll];
GO
CREATE PROCEDURE [dbo].[SMS_TinNhan_AdministrativeUnit_MapDeleteAll]
AS
BEGIN
	DELETE [SMS_TinNhan_AdministrativeUnit_Map]
END
GO
/* End SMS_TinNhan_AdministrativeUnit_MapDeleteAll */
