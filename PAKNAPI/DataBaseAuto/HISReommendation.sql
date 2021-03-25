
/* Start HIS_ReommendationGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationGetByID]
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
	FROM [HIS_Reommendation]
	WHERE [Id] = @Id
END
GO
/* End HIS_ReommendationGetByID */

/* Start HIS_ReommendationGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationGetAll]
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
	FROM [HIS_Reommendation]
END
GO
/* End HIS_ReommendationGetAll */

/* Start HIS_ReommendationGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationGetAllOnPage]
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
	FROM [HIS_Reommendation]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_ReommendationGetAllOnPage */

/* Start HIS_ReommendationInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationInsert];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_Reommendation]
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
/* End HIS_ReommendationGetByID */

/* Start HIS_ReommendationUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_Reommendation] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_ReommendationUpdate */

/* Start HIS_ReommendationDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationDelete];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_Reommendation]
	WHERE [Id] = @Id
END
GO
/* End HIS_ReommendationDelete */

/* Start HIS_ReommendationDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_ReommendationDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_ReommendationDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_ReommendationDeleteAll]
AS
BEGIN
	DELETE [HIS_Reommendation]
END
GO
/* End HIS_ReommendationDeleteAll */
