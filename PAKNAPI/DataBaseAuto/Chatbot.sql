IF EXISTS( SELECT * FROM sys.tables WHERE name = 'SY_Chatbot' )
BEGIN
    DROP TABLE [dbo].[SY_Chatbot];
END

CREATE TABLE [dbo].[SY_Chatbot] (
    [Id] BIGINT         	IDENTITY (1, 1) NOT NULL,
    [Question]      		NVARCHAR (Max) NOT NULL,
	[Answer]				NVARCHAR (Max) NOT NULL,
	[CategoryId] 			INT        ,
	[IsActived] 			BIT NOT NULL DEFAULT (0),
	[IsDeleted] 			BIT NOT NULL DEFAULT (0),
    CONSTRAINT [PK_SY_Chatbot] PRIMARY KEY CLUSTERED ([Id] ASC)
)

/* Start ChatbotGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotGetByID];
GO
CREATE PROCEDURE [dbo].[ChatbotGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Question] ,
		[Answer] ,
		[CategoryId] ,
		[IsActived],
		[IsDeleted]
	FROM [SY_Chatbot]
	WHERE [Id] = @Id
END
GO
/* End ChatbotGetByID */

/* Start ChatbotGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotGetAll];
GO
CREATE PROCEDURE [dbo].[ChatbotGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Question] ,
		[IsActived],
		[IsDeleted],
		[Answer]
	FROM [SY_Chatbot]
END
GO
/* End ChatbotGetAll */

/* Start ChatbotGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[ChatbotGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null,
	@Question nvarchar(256) = null,
	@Answer nvarchar(256) = null,
	@IsActived bit = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Question] ,
		[Answer] ,
		[CategoryId] ,
		[IsActived],
		[IsDeleted]
		
	FROM [SY_Chatbot] SYCB
	WHERE 
		(ISNULL(@Question,'') = '' OR (SYCB.[Question] like N'%' + @Question + N'%')) AND  
		(ISNULL(@Answer,'') = '' OR (SYCB.[Answer] like N'%' + @Answer + N'%')) AND 
		(@IsActived IS NULL OR (SYCB.[IsActived] = @IsActived))
	ORDER BY [Id] DESC
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End ChatbotGetAllOnPage */

/* Start ChatbotInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotInsert];
GO
CREATE PROCEDURE [dbo].[ChatbotInsert]
	@Question nvarchar(1000) = null,
	@Answer nvarchar(1000) = null,
	@CategoryId int = 0,
	@IsActived bit = null,
	@IsDeleted bit = null
	
AS
BEGIN
	INSERT INTO [SY_Chatbot]
	(
		[Question] ,
		[Answer] ,
		[CategoryId] ,
		[IsActived],
		[IsDeleted]
		
	)
	VALUES
	(
		@Question,
		@Answer,
		@CategoryId,
		@IsActived,
		@IsDeleted
		
	)
END
GO
/* End ChatbotInsert */


/* Start ChatbotUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotUpdate];
GO
CREATE PROCEDURE [dbo].[ChatbotUpdate]
	@Id int = null,
	@Question nvarchar(1000) = null,
	@Answer nvarchar(1000) = null,
	@CategoryId int = null,
	@IsActived bit = null,
	@IsDeleted bit = null
	
AS
BEGIN
	UPDATE [SY_Chatbot] SET
		[Question] = @Question,
		[Answer] = @Answer,
		[CategoryId] = @CategoryId,
		[IsActived] = @IsActived,
		[IsDeleted] = @IsDeleted
	WHERE [Id] = @Id
END
GO
/* End ChatbotUpdate */

/* Start ChatbotDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotDelete];
GO
CREATE PROCEDURE [dbo].[ChatbotDelete]
	@Id int = null
AS
BEGIN
	DELETE [SY_Chatbot]
	WHERE [Id] = @Id
END
GO
/* End ChatbotDelete */

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

/* Start ChatbotCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotCount];
GO
CREATE PROCEDURE [dbo].[ChatbotCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Chatbot]
END
GO
/* End ChatbotCount */

/* Start ChatbotCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotCount];
GO
CREATE PROCEDURE [dbo].[ChatbotCount]
AS
BEGIN
	SELECT Count(*)
	FROM [SY_Chatbot]
END
GO
/* End ChatbotCount */

/* Start ChatbotGetNextCategoryId */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[ChatbotGetNextCategoryId]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [ChatbotGetNextCategoryId];
GO
CREATE PROCEDURE [dbo].[ChatbotGetNextCategoryId]
AS
BEGIN
	SELECT 
	(ISNULL(MAX(CategoryId),0) + 1  ) AS Max_CategoryId
	FROM [SY_Chatbot]
END
GO
/* End ChatbotGetNextCategoryId */

/* Start CREATE TABLE [dbo].[SY_DataChatbot] */
IF EXISTS( SELECT * FROM sys.tables WHERE name = 'SY_DataChatbot' )
BEGIN
    DROP TABLE [dbo].[SY_DataChatbot]
END

CREATE TABLE [dbo].[SY_DataChatbot] (
    [Id] BIGINT         	IDENTITY (1, 1) NOT NULL,
	[Kluid]					NVARCHAR (255) DEFAULT '',
	[UserId]				BIGINT,
	[FullName]      		NVARCHAR (255) NOT NULL,
    [Question]      		NVARCHAR (1000) NOT NULL,
	[Answer]				NVARCHAR (1000) NOT NULL
    CONSTRAINT [PK_SY_DataChatbot] PRIMARY KEY CLUSTERED ([Id] ASC)
)
/* End CREATE TABLE [dbo].[SY_DataChatbot] */

/* Start DataChatbotInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DataChatbotInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DataChatbotInsert];
GO
CREATE PROCEDURE [dbo].[DataChatbotInsert]
	@Kluid		NVARCHAR(255) = NULL,
	@UserId		BIGINT = NULL,
	@FullName    NVARCHAR(255) = NULL,
	@Question NVARCHAR(1000) = null,
	@Answer NVARCHAR(1000) = null
AS
BEGIN
	INSERT INTO [SY_DataChatbot]
	(
		[Kluid],
		[UserId],
		[FullName],
		[Question],
		[Answer]
	)
	VALUES
	(
		@Kluid,
		@UserId,
		@FullName,
		@Question,
		@Answer
	)
END
GO
/* End DataChatbotInsert */

/* Start HistoryChatbotGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HistoryChatbotGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HistoryChatbotGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HistoryChatbotGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null,
	@FullName nvarchar(256) = null,
	@Question nvarchar(256) = null,
	@Answer nvarchar(256) = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Kluid],
		[UserId],
		[FullName] ,
		[Question] ,
		[Answer]
	FROM [SY_DataChatbot] SYCB
	WHERE 
		(ISNULL(@FullName,'') = '' OR (SYCB.[FullName] like N'%' + @FullName + N'%')) AND
		(ISNULL(@Question,'') = '' OR (SYCB.[Question] like N'%' + @Question + N'%')) AND  
		(ISNULL(@Answer,'') = '' OR (SYCB.[Answer] like N'%' + @Answer + N'%')) 
	ORDER BY [Id] DESC
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HistoryChatbotGetAllOnPage */