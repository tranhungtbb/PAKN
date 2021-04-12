
/* Start MR_RecommendationKNCTGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTGetByID];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[RecommendationKNCTId],
		[CreatedDate],
		[SendDate],
		[EndDate],
		[District],
		[Code],
		[Content],
		[Classify],
		[Term],
		[FieldId],
		[Place],
		[Department],
		[Progress],
		[Status]
	FROM [MR_RecommendationKNCT]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCTGetByID */

/* Start MR_RecommendationKNCTGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTGetAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTGetAll]
AS
BEGIN
	SELECT
		[Id],
		[RecommendationKNCTId],
		[CreatedDate],
		[SendDate],
		[EndDate],
		[District],
		[Code],
		[Content],
		[Classify],
		[Term],
		[FieldId],
		[Place],
		[Department],
		[Progress],
		[Status]
	FROM [MR_RecommendationKNCT]
END
GO
/* End MR_RecommendationKNCTGetAll */

/* Start MR_RecommendationKNCTGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[RecommendationKNCTId],
		[CreatedDate],
		[SendDate],
		[EndDate],
		[District],
		[Code],
		[Content],
		[Classify],
		[Term],
		[FieldId],
		[Place],
		[Department],
		[Progress],
		[Status]
	FROM [MR_RecommendationKNCT]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_RecommendationKNCTGetAllOnPage */

/* Start MR_RecommendationKNCTInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTInsert];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTInsert]
	@RecommendationKNCTId int = null,
	@CreatedDate datetime = null,
	@SendDate datetime = null,
	@EndDate datetime = null,
	@District nvarchar(100) = null,
	@Code nvarchar(100) = null,
	@Content nvarchar(500) = null,
	@Classify nvarchar(200) = null,
	@Term nvarchar(500) = null,
	@FieldId int = null,
	@Place nvarchar(200) = null,
	@Department nvarchar(200) = null,
	@Progress ntext = null,
	@Status int = null
AS
BEGIN
	INSERT INTO [MR_RecommendationKNCT]
	(
		[RecommendationKNCTId],
		[CreatedDate],
		[SendDate],
		[EndDate],
		[District],
		[Code],
		[Content],
		[Classify],
		[Term],
		[FieldId],
		[Place],
		[Department],
		[Progress],
		[Status]
	)
	VALUES
	(
		@RecommendationKNCTId,
		@CreatedDate,
		@SendDate,
		@EndDate,
		@District,
		@Code,
		@Content,
		@Classify,
		@Term,
		@FieldId,
		@Place,
		@Department,
		@Progress,
		@Status
	)
END
GO
/* End MR_RecommendationKNCTGetByID */

/* Start MR_RecommendationKNCTUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTUpdate];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTUpdate]
	@Id int = null,
	@RecommendationKNCTId int = null,
	@CreatedDate datetime = null,
	@SendDate datetime = null,
	@EndDate datetime = null,
	@District nvarchar(100) = null,
	@Code nvarchar(100) = null,
	@Content nvarchar(500) = null,
	@Classify nvarchar(200) = null,
	@Term nvarchar(500) = null,
	@FieldId int = null,
	@Place nvarchar(200) = null,
	@Department nvarchar(200) = null,
	@Progress ntext = null,
	@Status int = null
AS
BEGIN
	UPDATE [MR_RecommendationKNCT] SET
		[RecommendationKNCTId] = @RecommendationKNCTId,
		[CreatedDate] = @CreatedDate,
		[SendDate] = @SendDate,
		[EndDate] = @EndDate,
		[District] = @District,
		[Code] = @Code,
		[Content] = @Content,
		[Classify] = @Classify,
		[Term] = @Term,
		[FieldId] = @FieldId,
		[Place] = @Place,
		[Department] = @Department,
		[Progress] = @Progress,
		[Status] = @Status
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCTUpdate */

/* Start MR_RecommendationKNCTDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTDelete];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_RecommendationKNCT]
	WHERE [Id] = @Id
END
GO
/* End MR_RecommendationKNCTDelete */

/* Start MR_RecommendationKNCTDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_RecommendationKNCTDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_RecommendationKNCTDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_RecommendationKNCTDeleteAll]
AS
BEGIN
	DELETE [MR_RecommendationKNCT]
END
GO
/* End MR_RecommendationKNCTDeleteAll */
