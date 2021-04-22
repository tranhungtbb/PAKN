
/* Start SY_User_Role_MapGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapGetByID];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapGetByID]
	@UserId int = null
AS
BEGIN
	SELECT
		[UserId],
		[RoleId]
	FROM [SY_User_Role_Map]
	WHERE [UserId] = @UserId
END
GO
/* End SY_User_Role_MapGetByID */

/* Start SY_User_Role_MapGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapGetAll];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapGetAll]
AS
BEGIN
	SELECT
		[UserId],
		[RoleId]
	FROM [SY_User_Role_Map]
END
GO
/* End SY_User_Role_MapGetAll */

/* Start SY_User_Role_MapGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[UserId],
		[RoleId]
	FROM [SY_User_Role_Map]
	ORDER BY [UserId,RoleId]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_User_Role_MapGetAllOnPage */

/* Start SY_User_Role_MapInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapInsert];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapInsert]
AS
BEGIN
	INSERT INTO [SY_User_Role_Map]
	(
	)
	VALUES
	(
	)
END
GO
/* End SY_User_Role_MapGetByID */

/* Start SY_User_Role_MapUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapUpdate];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapUpdate]
	@UserId int = null,
	@RoleId int = null
AS
BEGIN
	UPDATE [SY_User_Role_Map] SET
	WHERE [UserId] = @UserId
END
GO
/* End SY_User_Role_MapUpdate */

/* Start SY_User_Role_MapDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapDelete];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapDelete]
	@UserId int = null
AS
BEGIN
	DELETE [SY_User_Role_Map]
	WHERE [UserId] = @UserId
END
GO
/* End SY_User_Role_MapDelete */

/* Start SY_User_Role_MapDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_User_Role_MapDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_User_Role_MapDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_User_Role_MapDeleteAll]
AS
BEGIN
	DELETE [SY_User_Role_Map]
END
GO
/* End SY_User_Role_MapDeleteAll */
