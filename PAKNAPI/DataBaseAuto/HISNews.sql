
/* Start HIS_NewsGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_NewsGetByID]
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
	FROM [HIS_News]
	WHERE [Id] = @Id
END
GO
/* End HIS_NewsGetByID */

/* Start HIS_NewsGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_NewsGetAll]
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
	FROM [HIS_News]
END
GO
/* End HIS_NewsGetAll */

/* Start HIS_NewsGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_NewsGetAllOnPage]
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
	FROM [HIS_News]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_NewsGetAllOnPage */

/* Start HIS_NewsInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsInsert];
GO
CREATE PROCEDURE [dbo].[HIS_NewsInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_News]
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
/* End HIS_NewsGetByID */

/* Start HIS_NewsUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_NewsUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_News] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_NewsUpdate */

/* Start HIS_NewsDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsDelete];
GO
CREATE PROCEDURE [dbo].[HIS_NewsDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_News]
	WHERE [Id] = @Id
END
GO
/* End HIS_NewsDelete */

/* Start HIS_NewsDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_NewsDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_NewsDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_NewsDeleteAll]
AS
BEGIN
	DELETE [HIS_News]
END
GO
/* End HIS_NewsDeleteAll */
