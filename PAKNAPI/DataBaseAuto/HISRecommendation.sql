
/* Start HIS_RecommendationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Recommendation]
	WHERE [Id] = @Id
END
GO
/* End HIS_RecommendationGetByID */

/* Start HIS_RecommendationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationGetAll]
AS
BEGIN
	SELECT
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Recommendation]
END
GO
/* End HIS_RecommendationGetAll */

/* Start HIS_RecommendationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	FROM [HIS_Recommendation]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_RecommendationGetAllOnPage */

/* Start HIS_RecommendationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationInsert];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_Recommendation]
	(
		[ObjectId],
		[Type],
		[Content],
		[Status],
		[CreatedBy],
		[CreatedDate]
	)
	VALUES
	(
		@ObjectId,
		@Type,
		@Content,
		@Status,
		@CreatedBy,
		@CreatedDate
	)
END
GO
/* End HIS_RecommendationGetByID */

/* Start HIS_RecommendationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_Recommendation] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_RecommendationUpdate */

/* Start HIS_RecommendationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationDelete];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_Recommendation]
	WHERE [Id] = @Id
END
GO
/* End HIS_RecommendationDelete */

/* Start HIS_RecommendationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_RecommendationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_RecommendationDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_RecommendationDeleteAll]
AS
BEGIN
	DELETE [HIS_Recommendation]
END
GO
/* End HIS_RecommendationDeleteAll */
