
/* Start CA_HashtagGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagGetByID];
GO
CREATE PROCEDURE [dbo].[CA_HashtagGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived]
	FROM [CA_Hashtag]
	WHERE [Id] = @Id
END
GO
/* End CA_HashtagGetByID */

/* Start CA_HashtagGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagGetAll];
GO
CREATE PROCEDURE [dbo].[CA_HashtagGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Name],
		[IsActived]
	FROM [CA_Hashtag]
END
GO
/* End CA_HashtagGetAll */

/* Start CA_HashtagGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[CA_HashtagGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Name],
		[IsActived]
	FROM [CA_Hashtag]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End CA_HashtagGetAllOnPage */

/* Start CA_HashtagInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagInsert];
GO
CREATE PROCEDURE [dbo].[CA_HashtagInsert]
	@Name nvarchar(50) = null,
	@IsActived bit = null
AS
BEGIN
	INSERT INTO [CA_Hashtag]
	(
		[Name],
		[IsActived]
	)
	VALUES
	(
		@Name,
		@IsActived
	)
END
GO
/* End CA_HashtagGetByID */

/* Start CA_HashtagUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagUpdate];
GO
CREATE PROCEDURE [dbo].[CA_HashtagUpdate]
	@Id int = null,
	@Name nvarchar(50) = null,
	@IsActived bit = null
AS
BEGIN
	UPDATE [CA_Hashtag] SET
		[Name] = @Name,
		[IsActived] = @IsActived
	WHERE [Id] = @Id
END
GO
/* End CA_HashtagUpdate */

/* Start CA_HashtagDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagDelete];
GO
CREATE PROCEDURE [dbo].[CA_HashtagDelete]
	@Id int = null
AS
BEGIN
	DELETE [CA_Hashtag]
	WHERE [Id] = @Id
END
GO
/* End CA_HashtagDelete */

/* Start CA_HashtagDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagDeleteAll];
GO
CREATE PROCEDURE [dbo].[CA_HashtagDeleteAll]
AS
BEGIN
	DELETE [CA_Hashtag]
END
GO
/* End CA_HashtagDeleteAll */

/* Start CA_HashtagCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[CA_HashtagCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [CA_HashtagCount];
GO
CREATE PROCEDURE [dbo].[CA_HashtagCount]
AS
BEGIN
	SELECT Count(*)
	FROM [CA_Hashtag]
END
GO
/* End CA_HashtagCount */
