
/* Start HIS_SMSGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSGetByID];
GO
CREATE PROCEDURE [dbo].[HIS_SMSGetByID]
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
	FROM [HIS_SMS]
	WHERE [Id] = @Id
END
GO
/* End HIS_SMSGetByID */

/* Start HIS_SMSGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSGetAll];
GO
CREATE PROCEDURE [dbo].[HIS_SMSGetAll]
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
	FROM [HIS_SMS]
END
GO
/* End HIS_SMSGetAll */

/* Start HIS_SMSGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[HIS_SMSGetAllOnPage]
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
	FROM [HIS_SMS]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End HIS_SMSGetAllOnPage */

/* Start HIS_SMSInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSInsert];
GO
CREATE PROCEDURE [dbo].[HIS_SMSInsert]
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	INSERT INTO [HIS_SMS]
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
/* End HIS_SMSGetByID */

/* Start HIS_SMSUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSUpdate];
GO
CREATE PROCEDURE [dbo].[HIS_SMSUpdate]
	@Id int = null,
	@ObjectId int = null,
	@Type int = null,
	@Content nvarchar(500) = null,
	@Status tinyint = null,
	@CreatedBy bigint = null,
	@CreatedDate datetime = null
AS
BEGIN
	UPDATE [HIS_SMS] SET
		[ObjectId] = @ObjectId,
		[Type] = @Type,
		[Content] = @Content,
		[Status] = @Status,
		[CreatedBy] = @CreatedBy,
		[CreatedDate] = @CreatedDate
	WHERE [Id] = @Id
END
GO
/* End HIS_SMSUpdate */

/* Start HIS_SMSDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSDelete];
GO
CREATE PROCEDURE [dbo].[HIS_SMSDelete]
	@Id int = null
AS
BEGIN
	DELETE [HIS_SMS]
	WHERE [Id] = @Id
END
GO
/* End HIS_SMSDelete */

/* Start HIS_SMSDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSDeleteAll];
GO
CREATE PROCEDURE [dbo].[HIS_SMSDeleteAll]
AS
BEGIN
	DELETE [HIS_SMS]
END
GO
/* End HIS_SMSDeleteAll */

/* Start HIS_SMSCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[HIS_SMSCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [HIS_SMSCount];
GO
CREATE PROCEDURE [dbo].[HIS_SMSCount]
AS
BEGIN
	SELECT Count(*)
	FROM [HIS_SMS]
END
GO
/* End HIS_SMSCount */
