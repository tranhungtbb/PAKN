
/* Start MR_RecommendationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationGetByID];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[Code],
		[Title],
		[Content],
		[Field],
		[UnitId],
		[TypeObject],
		[SendId],
		[Name],
		[Status],
		[SendDate],
		[ReactionaryWord],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate]
	FROM [MR_Recommendation]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationGetByID */

/* Start MR_RecommendationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationGetAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[Code],
		[Title],
		[Content],
		[Field],
		[UnitId],
		[TypeObject],
		[SendId],
		[Name],
		[Status],
		[SendDate],
		[ReactionaryWord],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate]
	FROM [MR_Recommendation]
END
GO
/* End MR_RecommendationGetAll */

/* Start MR_RecommendationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[Code],
		[Title],
		[Content],
		[Field],
		[UnitId],
		[TypeObject],
		[SendId],
		[Name],
		[Status],
		[SendDate],
		[ReactionaryWord],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate]
	FROM [MR_Recommendation]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_RecommendationGetAllOnPage */

/* Start MR_RecommendationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationInsert];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationInsert]
	@Code nvarchar(50) = null,
	@Title nvarchar(500) = null,
	@Content ntext = null,
	@Field int = null,
	@UnitId int = null,
	@TypeObject smallint = null,
	@SendId bigint = null,
	@Name nvarchar(100) = null,
	@Status tinyint = null,
	@SendDate date = null,
	@ReactionaryWord bit = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null,
	@UpdatedBy bigint = null,
	@UpdatedDate datetime = null
AS
BEGIN
	INSERT INTO [MR_Recommendation]
	(
		[Code],
		[Title],
		[Content],
		[Field],
		[UnitId],
		[TypeObject],
		[SendId],
		[Name],
		[Status],
		[SendDate],
		[ReactionaryWord],
		[CreatedBy],
		[CreatedDate],
		[UpdatedBy],
		[UpdatedDate]
	)
	VALUES
	(
		@Code,
		@Title,
		@Content,
		@Field,
		@UnitId,
		@TypeObject,
		@SendId,
		@Name,
		@Status,
		@SendDate,
		@ReactionaryWord,
		@CreatedBy,
		@CreatedDate,
		@UpdatedBy,
		@UpdatedDate
	)
END
GO
/* End MR_RecommendationGetByID */

/* Start MR_RecommendationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationUpdate];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationUpdate]
	@Id int = null,
	@Code nvarchar(50) = null,
	@Title nvarchar(500) = null,
	@Content ntext = null,
	@Field int = null,
	@UnitId int = null,
	@TypeObject smallint = null,
	@SendId bigint = null,
	@Name nvarchar(100) = null,
	@Status tinyint = null,
	@SendDate date = null,
	@ReactionaryWord bit = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null,
	@UpdatedBy bigint = null,
	@UpdatedDate datetime = null
AS
BEGIN
	UPDATE [MR_Recommendation] SET
		[Code] = @Code,
		[Title] = @Title,
		[Content] = @Content,
		[Field] = @Field,
		[UnitId] = @UnitId,
		[TypeObject] = @TypeObject,
		[SendId] = @SendId,
		[Name] = @Name,
		[Status] = @Status,
		[SendDate] = @SendDate,
		[ReactionaryWord] = @ReactionaryWord,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedDate] = @UpdatedDate
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationUpdate */

/* Start MR_RecommendationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationDelete];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationDelete */

/* Start MR_RecommendationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation]
END
GO
/* End MR_RecommendationDeleteAll */
