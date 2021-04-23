
/* Start CA_FieldDAMGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMGetByID];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[ParentId],
		[FieldDAMId]
	FROM [CA_FieldDAM]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldDAMGetByID */

/* Start CA_FieldDAMGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMGetAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[ParentId],
		[FieldDAMId]
	FROM [CA_FieldDAM]
END
GO
/* End CA_FieldDAMGetAll */

/* Start CA_FieldDAMGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[ParentId],
		[FieldDAMId]
	FROM [CA_FieldDAM]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_FieldDAMGetAllOnPage */

/* Start CA_FieldDAMInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMInsert];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMInsert]
	@Name nvarchar(100) = null,
	@ParentId int = null,
	@FieldDAMId int = null
AS
BEGIN
	INSERT INTO [CA_FieldDAM]
	(
		[Name],
		[ParentId],
		[FieldDAMId]
	)
	VALUES
	(
		@Name,
		@ParentId,
		@FieldDAMId
	)
END
GO
/* End CA_FieldDAMGetByID */

/* Start CA_FieldDAMUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMUpdate];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@ParentId int = null,
	@FieldDAMId int = null
AS
BEGIN
	UPDATE [CA_FieldDAM] SET
		[Name] = @Name,
		[ParentId] = @ParentId,
		[FieldDAMId] = @FieldDAMId
	WHERE [Id] = @Id
END
GO
/* End CA_FieldDAMUpdate */

/* Start CA_FieldDAMDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMDelete];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_FieldDAM]
	WHERE [Id] = @Id
END
GO
/* End CA_FieldDAMDelete */

/* Start CA_FieldDAMDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_FieldDAMDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_FieldDAMDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_FieldDAMDeleteAll]
AS
BEGIN
	DELETE [CA_FieldDAM]
END
GO
/* End CA_FieldDAMDeleteAll */
