
/* Start SY_DataChatbotGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotGetByID];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Question],
		[Answer],
		[UserId]
	FROM [SY_DataChatbot]
	WHERE [Id] = @Id
END
GO
/* End SY_DataChatbotGetByID */

/* Start SY_DataChatbotGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotGetAll];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Question],
		[Answer],
		[UserId]
	FROM [SY_DataChatbot]
END
GO
/* End SY_DataChatbotGetAll */

/* Start SY_DataChatbotGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Question],
		[Answer],
		[UserId]
	FROM [SY_DataChatbot]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_DataChatbotGetAllOnPage */

/* Start SY_DataChatbotInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotInsert];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotInsert]
	@Question nvarchar(1000) = null,
	@Answer nvarchar(1000) = null,
	@UserId uniqueidentifier = null
AS
BEGIN
	INSERT INTO [SY_DataChatbot]
	(
		[Question],
		[Answer],
		[UserId]
	)
	VALUES
	(
		@Question,
		@Answer,
		@UserId
	)
END
GO
/* End SY_DataChatbotGetByID */

/* Start SY_DataChatbotUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotUpdate];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotUpdate]
	@Id int = null,
	@Question nvarchar(1000) = null,
	@Answer nvarchar(1000) = null,
	@UserId uniqueidentifier = null
AS
BEGIN
	UPDATE [SY_DataChatbot] SET
		[Question] = @Question,
		[Answer] = @Answer,
		[UserId] = @UserId
	WHERE [Id] = @Id
END
GO
/* End SY_DataChatbotUpdate */

/* Start SY_DataChatbotDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotDelete];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_DataChatbot]
	WHERE [Id] = @Id
END
GO
/* End SY_DataChatbotDelete */

/* Start SY_DataChatbotDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_DataChatbotDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_DataChatbotDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_DataChatbotDeleteAll]
AS
BEGIN
	DELETE [SY_DataChatbot]
END
GO
/* End SY_DataChatbotDeleteAll */
