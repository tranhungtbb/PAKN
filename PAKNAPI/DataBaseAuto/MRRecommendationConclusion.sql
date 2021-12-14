
/* Start MR_Recommendation_ConclusionGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[UserCreatedId],
		[UnitCreatedId],
		[ReceiverId],
		[UnitReceiverId],
		[Content]
	FROM [MR_Recommendation_Conclusion]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ConclusionGetByID */

/* Start MR_Recommendation_ConclusionGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationId],
		[UserCreatedId],
		[UnitCreatedId],
		[ReceiverId],
		[UnitReceiverId],
		[Content]
	FROM [MR_Recommendation_Conclusion]
END
GO
/* End MR_Recommendation_ConclusionGetAll */

/* Start MR_Recommendation_ConclusionGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationId],
		[UserCreatedId],
		[UnitCreatedId],
		[ReceiverId],
		[UnitReceiverId],
		[Content]
	FROM [MR_Recommendation_Conclusion]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_ConclusionGetAllOnPage */

/* Start MR_Recommendation_ConclusionInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionInsert]
	@RecommendationId int = null,
	@UserCreatedId bigint = null,
	@UnitCreatedId int = null,
	@ReceiverId bigint = null,
	@UnitReceiverId int = null,
	@Content nvarchar(max) = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_Conclusion]
	(
		[RecommendationId],
		[UserCreatedId],
		[UnitCreatedId],
		[ReceiverId],
		[UnitReceiverId],
		[Content]
	)
	VALUES
	(
		@RecommendationId,
		@UserCreatedId,
		@UnitCreatedId,
		@ReceiverId,
		@UnitReceiverId,
		@Content
	)
END
GO
/* End MR_Recommendation_ConclusionGetByID */

/* Start MR_Recommendation_ConclusionUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionUpdate]
	@Id int = null,
	@RecommendationId int = null,
	@UserCreatedId bigint = null,
	@UnitCreatedId int = null,
	@ReceiverId bigint = null,
	@UnitReceiverId int = null,
	@Content nvarchar(max) = null
AS
BEGIN
	UPDATE [MR_Recommendation_Conclusion] SET
		[RecommendationId] = @RecommendationId,
		[UserCreatedId] = @UserCreatedId,
		[UnitCreatedId] = @UnitCreatedId,
		[ReceiverId] = @ReceiverId,
		[UnitReceiverId] = @UnitReceiverId,
		[Content] = @Content
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ConclusionUpdate */

/* Start MR_Recommendation_ConclusionDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_Conclusion]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_ConclusionDelete */

/* Start MR_Recommendation_ConclusionDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_Conclusion]
END
GO
/* End MR_Recommendation_ConclusionDeleteAll */

/* Start MR_Recommendation_ConclusionCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_ConclusionCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_ConclusionCount];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_ConclusionCount]
AS
BEGIN
	SELECT Count(*)
	FROM [MR_Recommendation_Conclusion]
END
GO
/* End MR_Recommendation_ConclusionCount */
