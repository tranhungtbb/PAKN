
/* Start SY_TimeGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeGetByID];
GO
CREATE PROCEDURE [dbo].[SY_TimeGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Time],
		[IsActived],
		[Description]
	FROM [SY_Time]
	WHERE [Id] = @Id
END
GO
/* End SY_TimeGetByID */

/* Start SY_TimeGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeGetAll];
GO
CREATE PROCEDURE [dbo].[SY_TimeGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[Time],
		[IsActived],
		[Description]
	FROM [SY_Time]
END
GO
/* End SY_TimeGetAll */

/* Start SY_TimeGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_TimeGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[Time],
		[IsActived],
		[Description]
	FROM [SY_Time]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_TimeGetAllOnPage */

/* Start SY_TimeInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeInsert];
GO
CREATE PROCEDURE [dbo].[SY_TimeInsert]
	@Name nvarchar(255) = null,
	@Time datetime = null,
	@IsActived bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	INSERT INTO [SY_Time]
	(
		[Name],
		[Time],
		[IsActived],
		[Description]
	)
	VALUES
	(
		@Name,
		@Time,
		@IsActived,
		@Description
	)
END
GO
/* End SY_TimeGetByID */

/* Start SY_TimeUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeUpdate];
GO
CREATE PROCEDURE [dbo].[SY_TimeUpdate]
	@Id int = null,
	@Name nvarchar(255) = null,
	@Time datetime = null,
	@IsActived bit = null,
	@Description nvarchar(1000) = null
AS
BEGIN
	UPDATE [SY_Time] SET
		[Name] = @Name,
		[Time] = @Time,
		[IsActived] = @IsActived,
		[Description] = @Description
	WHERE [Id] = @Id
END
GO
/* End SY_TimeUpdate */

/* Start SY_TimeDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeDelete];
GO
CREATE PROCEDURE [dbo].[SY_TimeDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_Time]
	WHERE [Id] = @Id
END
GO
/* End SY_TimeDelete */

/* Start SY_TimeDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_TimeDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_TimeDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_TimeDeleteAll]
AS
BEGIN
	DELETE [SY_Time]
END
GO
/* End SY_TimeDeleteAll */
