
/* Start CA_GroupWordGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordGetByID];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_GroupWord]
	WHERE [Id] = @Id
END
GO
/* End CA_GroupWordGetByID */

/* Start CA_GroupWordGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordGetAll];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_GroupWord]
END
GO
/* End CA_GroupWordGetAll */

/* Start CA_GroupWordGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	FROM [CA_GroupWord]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_GroupWordGetAllOnPage */

/* Start CA_GroupWordInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordInsert];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordInsert]
	@Name nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [CA_GroupWord]
	(
		[Name],
		[IsActived],
		[IsDeleted],
		[Description]
	)
	VALUES
	(
		@Name,
		@IsActived,
		@IsDeleted,
		@Description
	)
END
GO
/* End CA_GroupWordGetByID */

/* Start CA_GroupWordUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordUpdate];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordUpdate]
	@Id int = null,
	@Name nvarchar(100) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [CA_GroupWord] SET
		[Name] = @Name,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End CA_GroupWordUpdate */

/* Start CA_GroupWordDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordDelete];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_GroupWord]
	WHERE [Id] = @Id
END
GO
/* End CA_GroupWordDelete */

/* Start CA_GroupWordDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_GroupWordDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_GroupWordDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_GroupWordDeleteAll]
AS
BEGIN
	DELETE [CA_GroupWord]
END
GO
/* End CA_GroupWordDeleteAll */
