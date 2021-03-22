
/* Start SY_PermissionGroupUserGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserGetByID]
	@PermissionId smallint = null
AS
BEGIN
	SELECT
		[PermissionId],
		[GroupUserId]
	FROM [SY_PermissionGroupUser]
	WHERE [PermissionId] = @PermissionId
END
GO
/* End SY_PermissionGroupUserGetByID */

/* Start SY_PermissionGroupUserGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserGetAll]
AS
BEGIN
	SELECT
		[PermissionId],
		[GroupUserId]
	FROM [SY_PermissionGroupUser]
END
GO
/* End SY_PermissionGroupUserGetAll */

/* Start SY_PermissionGroupUserGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[PermissionId],
		[GroupUserId]
	FROM [SY_PermissionGroupUser]
	ORDER BY [PermissionId,GroupUserId]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionGroupUserGetAllOnPage */

/* Start SY_PermissionGroupUserInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserInsert]
AS
BEGIN
	INSERT INTO [SY_PermissionGroupUser]
	(
	)
	VALUES
	(
	)
END
GO
/* End SY_PermissionGroupUserGetByID */

/* Start SY_PermissionGroupUserUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserUpdate]
	@PermissionId smallint = null,
	@GroupUserId smallint = null
AS
BEGIN
	UPDATE [SY_PermissionGroupUser] SET
	WHERE [PermissionId] = @PermissionId
END
GO
/* End SY_PermissionGroupUserUpdate */

/* Start SY_PermissionGroupUserDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserDelete]
	@PermissionId smallint = null
AS
BEGIN
	DELETE [SY_PermissionGroupUser]
	WHERE [PermissionId] = @PermissionId
END
GO
/* End SY_PermissionGroupUserDelete */

/* Start SY_PermissionGroupUserDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionGroupUserDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionGroupUserDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionGroupUserDeleteAll]
AS
BEGIN
	DELETE [SY_PermissionGroupUser]
END
GO
/* End SY_PermissionGroupUserDeleteAll */
