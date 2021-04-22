
/* Start SY_PermissionAPIGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIGetByID];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[PermissionId],
		[APIId]
	FROM [SY_PermissionAPI]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionAPIGetByID */

/* Start SY_PermissionAPIGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIGetAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIGetAll]
AS
BEGIN
	SELECT
		[Id],
		[PermissionId],
		[APIId]
	FROM [SY_PermissionAPI]
END
GO
/* End SY_PermissionAPIGetAll */

/* Start SY_PermissionAPIGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[PermissionId],
		[APIId]
	FROM [SY_PermissionAPI]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_PermissionAPIGetAllOnPage */

/* Start SY_PermissionAPIInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIInsert];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIInsert]
	@PermissionId int = null,
	@APIId int = null
AS
BEGIN
	INSERT INTO [SY_PermissionAPI]
	(
		[PermissionId],
		[APIId]
	)
	VALUES
	(
		@PermissionId,
		@APIId
	)
END
GO
/* End SY_PermissionAPIGetByID */

/* Start SY_PermissionAPIUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIUpdate];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIUpdate]
	@Id int = null,
	@PermissionId int = null,
	@APIId int = null
AS
BEGIN
	UPDATE [SY_PermissionAPI] SET
		[PermissionId] = @PermissionId,
		[APIId] = @APIId
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionAPIUpdate */

/* Start SY_PermissionAPIDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIDelete];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_PermissionAPI]
	WHERE [Id] = @Id
END
GO
/* End SY_PermissionAPIDelete */

/* Start SY_PermissionAPIDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_PermissionAPIDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_PermissionAPIDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_PermissionAPIDeleteAll]
AS
BEGIN
	DELETE [SY_PermissionAPI]
END
GO
/* End SY_PermissionAPIDeleteAll */
