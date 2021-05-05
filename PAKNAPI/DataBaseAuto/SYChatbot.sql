
/* Start SY_ChatbotGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotGetByID];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotGetByID]
	@Id bigint = null
AS
BEGIN
	SELECT
		[Id],
		[Question],
		[Answer],
		[IsActived],
		[IsDeleted],
		[CategoryId]
	FROM [SY_Chatbot]
	WHERE [Id] = @Id
END
GO
/* End SY_ChatbotGetByID */

/* Start SY_ChatbotGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotGetAll];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Question],
		[Answer],
		[IsActived],
		[IsDeleted],
		[CategoryId]
	FROM [SY_Chatbot]
END
GO
/* End SY_ChatbotGetAll */

/* Start SY_ChatbotGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Question],
		[Answer],
		[IsActived],
		[IsDeleted],
		[CategoryId]
	FROM [SY_Chatbot]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End SY_ChatbotGetAllOnPage */

/* Start SY_ChatbotInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotInsert];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotInsert]
	@Question nvarchar(max) = null,
	@Answer nvarchar(max) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@CategoryId int = null
AS
BEGIN
	INSERT INTO [SY_Chatbot]
	(
		[Question],
		[Answer],
		[IsActived],
		[IsDeleted],
		[CategoryId]
	)
	VALUES
	(
		@Question,
		@Answer,
		@IsActived,
		@IsDeleted,
		@CategoryId
	)
END
GO
/* End SY_ChatbotGetByID */

/* Start SY_ChatbotUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotUpdate];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotUpdate]
	@Id bigint = null,
	@Question nvarchar(max) = null,
	@Answer nvarchar(max) = null,
	@IsActived bit = null,
	@IsDeleted bit = null,
	@CategoryId int = null
AS
BEGIN
	UPDATE [SY_Chatbot] SET
		[Question] = @Question,
		[Answer] = @Answer,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted,
		[CategoryId] = @CategoryId
	WHERE [Id] = @Id
END
GO
/* End SY_ChatbotUpdate */

/* Start SY_ChatbotDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotDelete];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotDelete]
	@Id bigint = null
AS
BEGIN
	DELETE [SY_Chatbot]
	WHERE [Id] = @Id
END
GO
/* End SY_ChatbotDelete */

/* Start SY_ChatbotDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[SY_ChatbotDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [SY_ChatbotDeleteAll];
GO
CREATE PROCEDURE [dbo].[SY_ChatbotDeleteAll]
AS
BEGIN
	DELETE [SY_Chatbot]
END
GO
/* End SY_ChatbotDeleteAll */
