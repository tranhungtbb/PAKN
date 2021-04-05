
/* Start SY_UserGroupUserGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserGetByID];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[GroupUserId]
	FROM [SY_UserGroupUser]
	WHERE [Id] = @Id
END
GO
/* End SY_UserGroupUserGetByID */

/* Start SY_UserGroupUserGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserGetAll];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserGetAll]
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[GroupUserId]
	FROM [SY_UserGroupUser]
END
GO
/* End SY_UserGroupUserGetAll */

/* Start SY_UserGroupUserGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[UserId],
		[GroupUserId]
	FROM [SY_UserGroupUser]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_UserGroupUserGetAllOnPage */

/* Start SY_UserGroupUserInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserInsert];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserInsert]
	@UserId int = null,
	@GroupUserId smallint = null
AS
BEGIN
	INSERT INTO [SY_UserGroupUser]
	(
		[UserId],
		[GroupUserId]
	)
	VALUES
	(
		@UserId,
		@GroupUserId
	)
END
GO
/* End SY_UserGroupUserGetByID */

/* Start SY_UserGroupUserUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserUpdate];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserUpdate]
	@Id int = null,
	@UserId int = null,
	@GroupUserId smallint = null
AS
BEGIN
	UPDATE [SY_UserGroupUser] SET
		[UserId] = @UserId,
		[GroupUserId] = @GroupUserId
	WHERE [Id] = @Id
END
GO
/* End SY_UserGroupUserUpdate */

/* Start SY_UserGroupUserDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserDelete];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_UserGroupUser]
	WHERE [Id] = @Id
END
GO
/* End SY_UserGroupUserDelete */

/* Start SY_UserGroupUserDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserDeleteAll]
AS
BEGIN
	DELETE [SY_UserGroupUser]
END
GO
/* End SY_UserGroupUserDeleteAll */

/* Start SY_UserGroupUserCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGroupUserCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGroupUserCount];
GO
CREATE PROCEDURE [dbo].[SY_UserGroupUserCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_UserGroupUser]
END
GO
/* End SY_UserGroupUserCount */
