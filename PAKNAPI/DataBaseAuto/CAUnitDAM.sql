
/* Start CA_UnitDAMGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMGetByID];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[UnitId],
		[Name]
	FROM [CA_UnitDAM]
	WHERE [Id] = @Id
END
GO
/* End CA_UnitDAMGetByID */

/* Start CA_UnitDAMGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMGetAll];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMGetAll]
AS
BEGIN
	SELECT
		[Id],
		[UnitId],
		[Name]
	FROM [CA_UnitDAM]
END
GO
/* End CA_UnitDAMGetAll */

/* Start CA_UnitDAMGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[UnitId],
		[Name]
	FROM [CA_UnitDAM]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_UnitDAMGetAllOnPage */

/* Start CA_UnitDAMInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMInsert];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMInsert]
	@UnitId int = null,
	@Name nvarchar(100) = null
AS
BEGIN
	INSERT INTO [CA_UnitDAM]
	(
		[UnitId],
		[Name]
	)
	VALUES
	(
		@UnitId,
		@Name
	)
END
GO
/* End CA_UnitDAMGetByID */

/* Start CA_UnitDAMUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMUpdate];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMUpdate]
	@Id int = null,
	@UnitId int = null,
	@Name nvarchar(100) = null
AS
BEGIN
	UPDATE [CA_UnitDAM] SET
		[UnitId] = @UnitId,
		[Name] = @Name
	WHERE [Id] = @Id
END
GO
/* End CA_UnitDAMUpdate */

/* Start CA_UnitDAMDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMDelete];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_UnitDAM]
	WHERE [Id] = @Id
END
GO
/* End CA_UnitDAMDelete */

/* Start CA_UnitDAMDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_UnitDAMDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_UnitDAMDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_UnitDAMDeleteAll]
AS
BEGIN
	DELETE [CA_UnitDAM]
END
GO
/* End CA_UnitDAMDeleteAll */
