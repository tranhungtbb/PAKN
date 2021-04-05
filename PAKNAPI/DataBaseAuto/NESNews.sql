
/* Start NES_NewsGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsGetByID];
GO
CREATE PROCEDURE [dbo].[NES_NewsGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[PostType],
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[NewsType],
		[ViewCount],
		[Url],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[PublishedBy],
		[PublishedDate],
		[WithdrawBy],
		[WithdrawDate]
	FROM [NES_News]
	WHERE [Id] = @Id
END
GO
/* End NES_NewsGetByID */

/* Start NES_NewsGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsGetAll];
GO
CREATE PROCEDURE [dbo].[NES_NewsGetAll]
AS
BEGIN
	SELECT
		[PostType],
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[NewsType],
		[ViewCount],
		[Url],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[PublishedBy],
		[PublishedDate],
		[WithdrawBy],
		[WithdrawDate]
	FROM [NES_News]
END
GO
/* End NES_NewsGetAll */

/* Start NES_NewsGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[NES_NewsGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[PostType],
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[NewsType],
		[ViewCount],
		[Url],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[PublishedBy],
		[PublishedDate],
		[WithdrawBy],
		[WithdrawDate]
	FROM [NES_News]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End NES_NewsGetAllOnPage */

/* Start NES_NewsInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsInsert];
GO
CREATE PROCEDURE [dbo].[NES_NewsInsert]
	@PostType bit = null,
	@IsPublished bit = null,
	@Status int = null,
	@Title nvarchar(500) = null,
	@Summary nvarchar(2000) = null,
	@Contents ntext = null,
	@ImagePath varchar(500) = null,
	@NewsType int = null,
	@ViewCount int = null,
	@Url nvarchar(500) = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@PublishedBy int = null,
	@PublishedDate datetime = null,
	@WithdrawBy int = null,
	@WithdrawDate date = null
AS
BEGIN
	INSERT INTO [NES_News]
	(
		[PostType],
		[IsPublished],
		[Status],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[NewsType],
		[ViewCount],
		[Url],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate],
		[PublishedBy],
		[PublishedDate],
		[WithdrawBy],
		[WithdrawDate]
	)
	VALUES
	(
		@PostType,
		@IsPublished,
		@Status,
		@Title,
		@Summary,
		@Contents,
		@ImagePath,
		@NewsType,
		@ViewCount,
		@Url,
		@CreatedBy,
		@CreatedDate,
		@UpdatedBy,
		@UpdatedDate,
		@PublishedBy,
		@PublishedDate,
		@WithdrawBy,
		@WithdrawDate
	)
END
GO
/* End NES_NewsGetByID */

/* Start NES_NewsUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsUpdate];
GO
CREATE PROCEDURE [dbo].[NES_NewsUpdate]
	@PostType bit = null,
	@IsPublished bit = null,
	@Status int = null,
	@Id int = null,
	@Title nvarchar(500) = null,
	@Summary nvarchar(2000) = null,
	@Contents ntext = null,
	@ImagePath varchar(500) = null,
	@NewsType int = null,
	@ViewCount int = null,
	@Url nvarchar(500) = null,
	@CreatedBy int = null,
	@CreatedDate datetime = null,
	@UpdatedBy int = null,
	@UpdatedDate datetime = null,
	@PublishedBy int = null,
	@PublishedDate datetime = null,
	@WithdrawBy int = null,
	@WithdrawDate date = null
AS
BEGIN
	UPDATE [NES_News] SET
		[PostType] = @PostType,
		[IsPublished] = @IsPublished,
		[Status] = @Status,
		[Title] = @Title,
		[Summary] = @Summary,
		[Contents] = @Contents,
		[ImagePath] = @ImagePath,
		[NewsType] = @NewsType,
		[ViewCount] = @ViewCount,
		[Url] = @Url,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedDate] = @UpdatedDate,
		[PublishedBy] = @PublishedBy,
		[PublishedDate] = @PublishedDate,
		[WithdrawBy] = @WithdrawBy,
		[WithdrawDate] = @WithdrawDate
	WHERE [Id] = @Id
END
GO
/* End NES_NewsUpdate */

/* Start NES_NewsDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsDelete];
GO
CREATE PROCEDURE [dbo].[NES_NewsDelete]
	@Id int = null
AS
BEGIN
	DELETE [NES_News]
	WHERE [Id] = @Id
END
GO
/* End NES_NewsDelete */

/* Start NES_NewsDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NES_NewsDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NES_NewsDeleteAll];
GO
CREATE PROCEDURE [dbo].[NES_NewsDeleteAll]
AS
BEGIN
	DELETE [NES_News]
END
GO
/* End NES_NewsDeleteAll */
