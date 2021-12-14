
/* Start DAM_ChargesGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Charges],
		[Description],
		[ChargesId]
	FROM [DAM_Charges]
	WHERE [Id] = @Id
END
GO
/* End DAM_ChargesGetByID */

/* Start DAM_ChargesGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesGetAll]
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Charges],
		[Description],
		[ChargesId]
	FROM [DAM_Charges]
END
GO
/* End DAM_ChargesGetAll */

/* Start DAM_ChargesGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[AdministrationId],
		[Charges],
		[Description],
		[ChargesId]
	FROM [DAM_Charges]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_ChargesGetAllOnPage */

/* Start DAM_ChargesInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesInsert];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesInsert]
	@AdministrationId int = null,
	@Charges nvarchar(10) = null,
	@Description nvarchar(max) = null,
	@ChargesId int = null
AS
BEGIN
	INSERT INTO [DAM_Charges]
	(
		[AdministrationId],
		[Charges],
		[Description],
		[ChargesId]
	)
	VALUES
	(
		@AdministrationId,
		@Charges,
		@Description,
		@ChargesId
	)
END
GO
/* End DAM_ChargesGetByID */

/* Start DAM_ChargesUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesUpdate]
	@Id int = null,
	@AdministrationId int = null,
	@Charges nvarchar(10) = null,
	@Description nvarchar(max) = null,
	@ChargesId int = null
AS
BEGIN
	UPDATE [DAM_Charges] SET
		[AdministrationId] = @AdministrationId,
		[Charges] = @Charges,
		[Description] = @Description,
		[ChargesId] = @ChargesId
	WHERE [Id] = @Id
END
GO
/* End DAM_ChargesUpdate */

/* Start DAM_ChargesDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesDelete];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_Charges]
	WHERE [Id] = @Id
END
GO
/* End DAM_ChargesDelete */

/* Start DAM_ChargesDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ChargesDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ChargesDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_ChargesDeleteAll]
AS
BEGIN
	DELETE [DAM_Charges]
END
GO
/* End DAM_ChargesDeleteAll */
