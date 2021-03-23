
/* Start SY_PermissionUserGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserGetByID]
	@UserId bigint = null
AS
BEGIN
	SELECT
		[UserId],
		[PermissionId],
		[FunctionId],
		[CategoryId]
	FROM [SY_PermissionUser]
	WHERE [UserId] = @UserId
END
GO
/* End SY_PermissionUserGetByID */

/* Start SY_PermissionUserGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserGetAll]
AS
BEGIN
	SELECT
		[UserId],
		[PermissionId],
		[FunctionId],
		[CategoryId]
	FROM [SY_PermissionUser]
END
GO
/* End SY_PermissionUserGetAll */

/* Start SY_PermissionUserGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[UserId],
		[PermissionId],
		[FunctionId],
		[CategoryId]
	FROM [SY_PermissionUser]
	ORDER BY [UserId,PermissionId]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionUserGetAllOnPage */

/* Start SY_PermissionUserInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserInsert]
	@FunctionId smallint = null,
	@CategoryId smallint = null
AS
BEGIN
	INSERT INTO [SY_PermissionUser]
	(
		[FunctionId],
		[CategoryId]
	)
	VALUES
	(
		@FunctionId,
		@CategoryId
	)
END
GO
/* End SY_PermissionUserGetByID */

/* Start SY_PermissionUserUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserUpdate]
	@UserId bigint = null,
	@PermissionId smallint = null,
	@FunctionId smallint = null,
	@CategoryId smallint = null
AS
BEGIN
	UPDATE [SY_PermissionUser] SET
		[FunctionId] = @FunctionId,
		[CategoryId] = @CategoryId
	WHERE [UserId] = @UserId
END
GO
/* End SY_PermissionUserUpdate */

/* Start SY_PermissionUserDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserDelete]
	@UserId bigint = null
AS
BEGIN
	DELETE [SY_PermissionUser]
	WHERE [UserId] = @UserId
END
GO
/* End SY_PermissionUserDelete */

/* Start SY_PermissionUserDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserDeleteAll]
AS
BEGIN
	DELETE [SY_PermissionUser]
END
GO
/* End SY_PermissionUserDeleteAll */

/* Start SY_PermissionUserCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionUserCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionUserCount];
GO
CREATE PROCEDURE [dbo].[SY_PermissionUserCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_PermissionUser]
END
GO
/* End SY_PermissionUserCount */
