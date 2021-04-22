
/* Start MR_Recommendation_GenCodeGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeGetByID];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[CurrentNumber],
		[Year]
	FROM [MR_Recommendation_GenCode]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GenCodeGetByID */

/* Start MR_Recommendation_GenCodeGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeGetAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeGetAll]
AS
BEGIN
	SELECT
		[Id],
		[CurrentNumber],
		[Year]
	FROM [MR_Recommendation_GenCode]
END
GO
/* End MR_Recommendation_GenCodeGetAll */

/* Start MR_Recommendation_GenCodeGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[CurrentNumber],
		[Year]
	FROM [MR_Recommendation_GenCode]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End MR_Recommendation_GenCodeGetAllOnPage */

/* Start MR_Recommendation_GenCodeInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeInsert];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeInsert]
	@CurrentNumber float = null,
	@Year int = null
AS
BEGIN
	INSERT INTO [MR_Recommendation_GenCode]
	(
		[CurrentNumber],
		[Year]
	)
	VALUES
	(
		@CurrentNumber,
		@Year
	)
END
GO
/* End MR_Recommendation_GenCodeGetByID */

/* Start MR_Recommendation_GenCodeUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeUpdate];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeUpdate]
	@Id int = null,
	@CurrentNumber float = null,
	@Year int = null
AS
BEGIN
	UPDATE [MR_Recommendation_GenCode] SET
		[CurrentNumber] = @CurrentNumber,
		[Year] = @Year
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GenCodeUpdate */

/* Start MR_Recommendation_GenCodeDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeDelete];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeDelete]
	@Id int = null
AS
BEGIN
	DELETE [MR_Recommendation_GenCode]
	WHERE [Id] = @Id
END
GO
/* End MR_Recommendation_GenCodeDelete */

/* Start MR_Recommendation_GenCodeDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeDeleteAll];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeDeleteAll]
AS
BEGIN
	DELETE [MR_Recommendation_GenCode]
END
GO
/* End MR_Recommendation_GenCodeDeleteAll */

/* Start MR_Recommendation_GenCodeCount */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[MR_Recommendation_GenCodeCount]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [MR_Recommendation_GenCodeCount];
GO
CREATE PROCEDURE [dbo].[MR_Recommendation_GenCodeCount]
AS
BEGIN
	SELECT Count(*)
	FROM [MR_Recommendation_GenCode]
END
GO
/* End MR_Recommendation_GenCodeCount */
