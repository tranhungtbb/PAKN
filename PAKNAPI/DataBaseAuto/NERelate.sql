
/* Start NE_RelateGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateGetByID];
GO
CREATE PROCEDURE [dbo].[NE_RelateGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[NewsId],
		[NewsIdRelate]
	FROM [NE_Relate]
	WHERE [Id] = @Id
END
GO
/* End NE_RelateGetByID */

/* Start NE_RelateGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateGetAll];
GO
CREATE PROCEDURE [dbo].[NE_RelateGetAll]
AS
BEGIN
	SELECT
		[Id],
		[NewsId],
		[NewsIdRelate]
	FROM [NE_Relate]
END
GO
/* End NE_RelateGetAll */

/* Start NE_RelateGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[NE_RelateGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[NewsId],
		[NewsIdRelate]
	FROM [NE_Relate]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End NE_RelateGetAllOnPage */

/* Start NE_RelateInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateInsert];
GO
CREATE PROCEDURE [dbo].[NE_RelateInsert]
	@NewsId int = null,
	@NewsIdRelate int = null
AS
BEGIN
	INSERT INTO [NE_Relate]
	(
		[NewsId],
		[NewsIdRelate]
	)
	VALUES
	(
		@NewsId,
		@NewsIdRelate
	)
END
GO
/* End NE_RelateGetByID */

/* Start NE_RelateUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateUpdate];
GO
CREATE PROCEDURE [dbo].[NE_RelateUpdate]
	@Id int = null,
	@NewsId int = null,
	@NewsIdRelate int = null
AS
BEGIN
	UPDATE [NE_Relate] SET
		[NewsId] = @NewsId,
		[NewsIdRelate] = @NewsIdRelate
	WHERE [Id] = @Id
END
GO
/* End NE_RelateUpdate */

/* Start NE_RelateDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateDelete];
GO
CREATE PROCEDURE [dbo].[NE_RelateDelete]
	@Id int = null
AS
BEGIN
	DELETE [NE_Relate]
	WHERE [Id] = @Id
END
GO
/* End NE_RelateDelete */

/* Start NE_RelateDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[NE_RelateDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [NE_RelateDeleteAll];
GO
CREATE PROCEDURE [dbo].[NE_RelateDeleteAll]
AS
BEGIN
	DELETE [NE_Relate]
END
GO
/* End NE_RelateDeleteAll */
