
/* Start SY_UserUnitGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitGetByID];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[UnitId],
		[PositionId],
		[IsMain]
	FROM [SY_UserUnit]
	WHERE [Id] = @Id
END
GO
/* End SY_UserUnitGetByID */

/* Start SY_UserUnitGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitGetAll];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitGetAll]
AS
BEGIN
	SELECT
		[Id],
		[UserId],
		[UnitId],
		[PositionId],
		[IsMain]
	FROM [SY_UserUnit]
END
GO
/* End SY_UserUnitGetAll */

/* Start SY_UserUnitGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[UserId],
		[UnitId],
		[PositionId],
		[IsMain]
	FROM [SY_UserUnit]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_UserUnitGetAllOnPage */

/* Start SY_UserUnitInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitInsert];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitInsert]
	@UserId int = null,
	@UnitId smallint = null,
	@PositionId smallint = null,
	@IsMain bit = null
AS
BEGIN
	INSERT INTO [SY_UserUnit]
	(
		[UserId],
		[UnitId],
		[PositionId],
		[IsMain]
	)
	VALUES
	(
		@UserId,
		@UnitId,
		@PositionId,
		@IsMain
	)
END
GO
/* End SY_UserUnitGetByID */

/* Start SY_UserUnitUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitUpdate];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitUpdate]
	@Id int = null,
	@UserId int = null,
	@UnitId smallint = null,
	@PositionId smallint = null,
	@IsMain bit = null
AS
BEGIN
	UPDATE [SY_UserUnit] SET
		[UserId] = @UserId,
		[UnitId] = @UnitId,
		[PositionId] = @PositionId,
		[IsMain] = @IsMain
	WHERE [Id] = @Id
END
GO
/* End SY_UserUnitUpdate */

/* Start SY_UserUnitDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitDelete];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_UserUnit]
	WHERE [Id] = @Id
END
GO
/* End SY_UserUnitDelete */

/* Start SY_UserUnitDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitDeleteAll]
AS
BEGIN
	DELETE [SY_UserUnit]
END
GO
/* End SY_UserUnitDeleteAll */

/* Start SY_UserUnitCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_UserUnitCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_UserUnitCount];
GO
CREATE PROCEDURE [dbo].[SY_UserUnitCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_UserUnit]
END
GO
/* End SY_UserUnitCount */
