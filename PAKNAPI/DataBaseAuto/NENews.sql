
/* Start NE_NewsGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsGetByID];
GO
CREATE PROCEDURE [dbo].[NE_NewsGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[PostType],
		[IsNotification],
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
	FROM [NE_News]
	WHERE [Id] = @Id
END
GO
/* End NE_NewsGetByID */

/* Start NE_NewsGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsGetAll];
GO
CREATE PROCEDURE [dbo].[NE_NewsGetAll]
AS
BEGIN
	SELECT
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[PostType],
		[IsNotification],
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
	FROM [NE_News]
END
GO
/* End NE_NewsGetAll */

/* Start NE_NewsGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[NE_NewsGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[IsPublished],
		[Status],
		[Id],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[PostType],
		[IsNotification],
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
	FROM [NE_News]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End NE_NewsGetAllOnPage */

/* Start NE_NewsInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsInsert];
GO
CREATE PROCEDURE [dbo].[NE_NewsInsert]
	@IsPublished bit = null,
	@Status int = null,
	@Title nvarchar(500) = null,
	@Summary nvarchar(2000) = null,
	@Contents ntext = null,
	@ImagePath varchar(500) = null,
	@PostType varchar(50) = null,
	@IsNotification bit = null,
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
	INSERT INTO [NE_News]
	(
		[IsPublished],
		[Status],
		[Title],
		[Summary],
		[Contents],
		[ImagePath],
		[PostType],
		[IsNotification],
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
		@IsPublished,
		@Status,
		@Title,
		@Summary,
		@Contents,
		@ImagePath,
		@PostType,
		@IsNotification,
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
/* End NE_NewsGetByID */

/* Start NE_NewsUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsUpdate];
GO
CREATE PROCEDURE [dbo].[NE_NewsUpdate]
	@IsPublished bit = null,
	@Status int = null,
	@Id int = null,
	@Title nvarchar(500) = null,
	@Summary nvarchar(2000) = null,
	@Contents ntext = null,
	@ImagePath varchar(500) = null,
	@PostType varchar(50) = null,
	@IsNotification bit = null,
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
	UPDATE [NE_News] SET
		[IsPublished] = @IsPublished,
		[Status] = @Status,
		[Title] = @Title,
		[Summary] = @Summary,
		[Contents] = @Contents,
		[ImagePath] = @ImagePath,
		[PostType] = @PostType,
		[IsNotification] = @IsNotification,
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
/* End NE_NewsUpdate */

/* Start NE_NewsDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsDelete];
GO
CREATE PROCEDURE [dbo].[NE_NewsDelete]
	@Id int = null
AS
BEGIN
	DELETE [NE_News]
	WHERE [Id] = @Id
END
GO
/* End NE_NewsDelete */

/* Start NE_NewsDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_NewsDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_NewsDeleteAll];
GO
CREATE PROCEDURE [dbo].[NE_NewsDeleteAll]
AS
BEGIN
	DELETE [NE_News]
END
GO
/* End NE_NewsDeleteAll */
