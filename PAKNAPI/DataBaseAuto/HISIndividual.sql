
/* Start HIS_IndividualGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualGetByID]
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
	FROM [HIS_Individual]
	WHERE [Id] = @Id
END
GO
/* End HIS_IndividualGetByID */

/* Start HIS_IndividualGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualGetAll]
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
	FROM [HIS_Individual]
END
GO
/* End HIS_IndividualGetAll */

/* Start HIS_IndividualGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualGetAllOnPage]
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
	FROM [HIS_Individual]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_IndividualGetAllOnPage */

/* Start HIS_IndividualInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualInsert];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_Individual]
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
/* End HIS_IndividualGetByID */

/* Start HIS_IndividualUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_Individual] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_IndividualUpdate */

/* Start HIS_IndividualDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualDelete];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_Individual]
	WHERE [Id] = @Id
END
GO
/* End HIS_IndividualDelete */

/* Start HIS_IndividualDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_IndividualDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_IndividualDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_IndividualDeleteAll]
AS
BEGIN
	DELETE [HIS_Individual]
END
GO
/* End HIS_IndividualDeleteAll */
