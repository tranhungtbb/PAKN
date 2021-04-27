
/* Start SY_UserGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGetByID];
GO
CREATE PROCEDURE [dbo].[SY_UserGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[TypeId],
		[FullName],
		[UserName],
		[Password],
		[Salt],
		[IsActived],
		[IsDeleted],
		[Gender],
		[Type],
		[IsSuperAdmin],
		[CountLock],
		[LockEndOut],
		[Avatar],
		[Address],
		[PositionId],
		[Email],
		[Phone],
		[UnitId]
	FROM [SY_User]
	WHERE [Id] = @Id
END
GO
/* End SY_UserGetByID */

/* Start SY_UserGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGetAll];
GO
CREATE PROCEDURE [dbo].[SY_UserGetAll]
AS
BEGIN
	SELECT
		[Id],
		[TypeId],
		[FullName],
		[UserName],
		[Password],
		[Salt],
		[IsActived],
		[IsDeleted],
		[Gender],
		[Type],
		[IsSuperAdmin],
		[CountLock],
		[LockEndOut],
		[Avatar],
		[Address],
		[PositionId],
		[Email],
		[Phone],
		[UnitId]
	FROM [SY_User]
END
GO
/* End SY_UserGetAll */

/* Start SY_UserGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_UserGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[TypeId],
		[FullName],
		[UserName],
		[Password],
		[Salt],
		[IsActived],
		[IsDeleted],
		[Gender],
		[Type],
		[IsSuperAdmin],
		[CountLock],
		[LockEndOut],
		[Avatar],
		[Address],
		[PositionId],
		[Email],
		[Phone],
		[UnitId]
	FROM [SY_User]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_UserGetAllOnPage */

/* Start SY_UserInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserInsert];
GO
CREATE PROCEDURE [dbo].[SY_UserInsert]
	@TypeId int = null,
	@FullName nvarchar(256) = null,
	@UserName nvarchar(100) = null,
	@Password nvarchar(500) = null,
	@Salt nvarchar(500) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Gender bit = null,
	@Type tinyint = null,
	@IsSuperAdmin bit = null,
	@CountLock tinyint = null,
	@LockEndOut datetime = null,
	@Avatar nvarchar(255) = null,
	@Address nvarchar(500) = null,
	@PositionId int = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@UnitId int = null
AS
BEGIN
	INSERT INTO [SY_User]
	(
		[TypeId],
		[FullName],
		[UserName],
		[Password],
		[Salt],
		[IsActived],
		[IsDeleted],
		[Gender],
		[Type],
		[IsSuperAdmin],
		[CountLock],
		[LockEndOut],
		[Avatar],
		[Address],
		[PositionId],
		[Email],
		[Phone],
		[UnitId]
	)
	VALUES
	(
		@TypeId,
		@FullName,
		@UserName,
		@Password,
		@Salt,
		@IsActived,
		@IsDeleted,
		@Gender,
		@Type,
		@IsSuperAdmin,
		@CountLock,
		@LockEndOut,
		@Avatar,
		@Address,
		@PositionId,
		@Email,
		@Phone,
		@UnitId
	)
END
GO
/* End SY_UserGetByID */

/* Start SY_UserUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUpdate];
GO
CREATE PROCEDURE [dbo].[SY_UserUpdate]
	@Id bigint = null,
	@TypeId int = null,
	@FullName nvarchar(256) = null,
	@UserName nvarchar(100) = null,
	@Password nvarchar(500) = null,
	@Salt nvarchar(500) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Gender bit = null,
	@Type tinyint = null,
	@IsSuperAdmin bit = null,
	@CountLock tinyint = null,
	@LockEndOut datetime = null,
	@Avatar nvarchar(255) = null,
	@Address nvarchar(500) = null,
	@PositionId int = null,
	@Email nvarchar(256) = null,
	@Phone varchar(11) = null,
	@UnitId int = null
AS
BEGIN
	UPDATE [SY_User] SET
		[TypeId] = @TypeId,
		[FullName] = @FullName,
		[UserName] = @UserName,
		[Password] = @Password,
		[Salt] = @Salt,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Gender] = @Gender,
		[Type] = @Type,
		[IsSuperAdmin] = @IsSuperAdmin,
		[CountLock] = @CountLock,
		[LockEndOut] = @LockEndOut,
		[Avatar] = @Avatar,
		[Address] = @Address,
		[PositionId] = @PositionId,
		[Email] = @Email,
		[Phone] = @Phone,
		[UnitId] = @UnitId
	WHERE [Id] = @Id
END
GO
/* End SY_UserUpdate */

/* Start SY_UserDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserDelete];
GO
CREATE PROCEDURE [dbo].[SY_UserDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_User]
	WHERE [Id] = @Id
END
GO
/* End SY_UserDelete */

/* Start SY_UserDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_UserDeleteAll]
AS
BEGIN
	DELETE [SY_User]
END
GO
/* End SY_UserDeleteAll */

/* Start SY_UserCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserCount];
GO
CREATE PROCEDURE [dbo].[SY_UserCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_User]
END
GO
/* End SY_UserCount */
