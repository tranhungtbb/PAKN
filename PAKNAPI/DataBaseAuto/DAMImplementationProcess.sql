
/* Start DAM_ImplementationProcessGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Name],
		[Unit],
		[Time],
		[Result]
	FROM [DAM_ImplementationProcess]
	WHERE [Id] = @Id
END
GO
/* End DAM_ImplementationProcessGetByID */

/* Start DAM_ImplementationProcessGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessGetAll]
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[Name],
		[Unit],
		[Time],
		[Result]
	FROM [DAM_ImplementationProcess]
END
GO
/* End DAM_ImplementationProcessGetAll */

/* Start DAM_ImplementationProcessGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[AdministrationId],
		[Name],
		[Unit],
		[Time],
		[Result]
	FROM [DAM_ImplementationProcess]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_ImplementationProcessGetAllOnPage */

/* Start DAM_ImplementationProcessInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessInsert];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessInsert]
	@AdministrationId int = null,
	@Name nvarchar(500) = null,
	@Unit nvarchar(500) = null,
	@Time nvarchar(10) = null,
	@Result nvarchar(max) = null
AS
BEGIN
	INSERT INTO [DAM_ImplementationProcess]
	(
		[AdministrationId],
		[Name],
		[Unit],
		[Time],
		[Result]
	)
	VALUES
	(
		@AdministrationId,
		@Name,
		@Unit,
		@Time,
		@Result
	)
END
GO
/* End DAM_ImplementationProcessGetByID */

/* Start DAM_ImplementationProcessUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessUpdate]
	@Id int = null,
	@AdministrationId int = null,
	@Name nvarchar(500) = null,
	@Unit nvarchar(500) = null,
	@Time nvarchar(10) = null,
	@Result nvarchar(max) = null
AS
BEGIN
	UPDATE [DAM_ImplementationProcess] SET
		[AdministrationId] = @AdministrationId,
		[Name] = @Name,
		[Unit] = @Unit,
		[Time] = @Time,
		[Result] = @Result
	WHERE [Id] = @Id
END
GO
/* End DAM_ImplementationProcessUpdate */

/* Start DAM_ImplementationProcessDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessDelete];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_ImplementationProcess]
	WHERE [Id] = @Id
END
GO
/* End DAM_ImplementationProcessDelete */

/* Start DAM_ImplementationProcessDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_ImplementationProcessDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_ImplementationProcessDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_ImplementationProcessDeleteAll]
AS
BEGIN
	DELETE [DAM_ImplementationProcess]
END
GO
/* End DAM_ImplementationProcessDeleteAll */
