
/* Start RM_RemindGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindGetByID];
GO
CREATE PROCEDURE [dbo].[RM_RemindGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[PetitionId],
		[Content]
	FROM [RM_Remind]
	WHERE [Id] = @Id
END
GO
/* End RM_RemindGetByID */

/* Start RM_RemindGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindGetAll];
GO
CREATE PROCEDURE [dbo].[RM_RemindGetAll]
AS
BEGIN
	SELECT
		[Id],
		[PetitionId],
		[Content]
	FROM [RM_Remind]
END
GO
/* End RM_RemindGetAll */

/* Start RM_RemindGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[RM_RemindGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[PetitionId],
		[Content]
	FROM [RM_Remind]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End RM_RemindGetAllOnPage */

/* Start RM_RemindInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindInsert];
GO
CREATE PROCEDURE [dbo].[RM_RemindInsert]
	@PetitionId int = null,
	@Content nvarchar(max) = null
AS
BEGIN
	INSERT INTO [RM_Remind]
	(
		[PetitionId],
		[Content]
	)
	VALUES
	(
		@PetitionId,
		@Content
	)
END
GO
/* End RM_RemindGetByID */

/* Start RM_RemindUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindUpdate];
GO
CREATE PROCEDURE [dbo].[RM_RemindUpdate]
	@Id int = null,
	@PetitionId int = null,
	@Content nvarchar(max) = null
AS
BEGIN
	UPDATE [RM_Remind] SET
		[PetitionId] = @PetitionId,
		[Content] = @Content
	WHERE [Id] = @Id
END
GO
/* End RM_RemindUpdate */

/* Start RM_RemindDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindDelete];
GO
CREATE PROCEDURE [dbo].[RM_RemindDelete]
	@Id int = null
AS
BEGIN
	DELETE [RM_Remind]
	WHERE [Id] = @Id
END
GO
/* End RM_RemindDelete */

/* Start RM_RemindDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[RM_RemindDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [RM_RemindDeleteAll];
GO
CREATE PROCEDURE [dbo].[RM_RemindDeleteAll]
AS
BEGIN
	DELETE [RM_Remind]
END
GO
/* End RM_RemindDeleteAll */
