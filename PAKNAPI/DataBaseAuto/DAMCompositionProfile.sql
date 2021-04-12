
/* Start DAM_CompositionProfileGetByID */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileGetByID]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileGetByID];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileGetByID]
	@Id int = null
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[NameExhibit],
		[OriginalForm],
		[CopyForm],
		[IsBind],
		[Form],
		[FormType]
	FROM [DAM_CompositionProfile]
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfileGetByID */

/* Start DAM_CompositionProfileGetAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileGetAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileGetAll];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileGetAll]
AS
BEGIN
	SELECT
		[Id],
		[AdministrationId],
		[NameExhibit],
		[OriginalForm],
		[CopyForm],
		[IsBind],
		[Form],
		[FormType]
	FROM [DAM_CompositionProfile]
END
GO
/* End DAM_CompositionProfileGetAll */

/* Start DAM_CompositionProfileGetAllOnPage */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileGetAllOnPage]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileGetAllOnPage];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileGetAllOnPage]
	@PageSize int = null,
	@PageIndex int = null
AS
BEGIN
	SELECT
		COUNT(*) OVER ( ORDER BY (SELECT NULL)) as RowNumber,
		[Id],
		[AdministrationId],
		[NameExhibit],
		[OriginalForm],
		[CopyForm],
		[IsBind],
		[Form],
		[FormType]
	FROM [DAM_CompositionProfile]
	ORDER BY [Id]
	OFFSET (@PageIndex-1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
/* End DAM_CompositionProfileGetAllOnPage */

/* Start DAM_CompositionProfileInsert */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileInsert]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileInsert];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileInsert]
	@AdministrationId int = null,
	@NameExhibit nvarchar(2000) = null,
	@OriginalForm tinyint = null,
	@CopyForm tinyint = null,
	@IsBind bit = null,
	@Form nvarchar(100) = null,
	@FormType nvarchar(50) = null
AS
BEGIN
	INSERT INTO [DAM_CompositionProfile]
	(
		[AdministrationId],
		[NameExhibit],
		[OriginalForm],
		[CopyForm],
		[IsBind],
		[Form],
		[FormType]
	)
	VALUES
	(
		@AdministrationId,
		@NameExhibit,
		@OriginalForm,
		@CopyForm,
		@IsBind,
		@Form,
		@FormType
	)
END
GO
/* End DAM_CompositionProfileGetByID */

/* Start DAM_CompositionProfileUpdate */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileUpdate]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileUpdate];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileUpdate]
	@Id int = null,
	@AdministrationId int = null,
	@NameExhibit nvarchar(2000) = null,
	@OriginalForm tinyint = null,
	@CopyForm tinyint = null,
	@IsBind bit = null,
	@Form nvarchar(100) = null,
	@FormType nvarchar(50) = null
AS
BEGIN
	UPDATE [DAM_CompositionProfile] SET
		[AdministrationId] = @AdministrationId,
		[NameExhibit] = @NameExhibit,
		[OriginalForm] = @OriginalForm,
		[CopyForm] = @CopyForm,
		[IsBind] = @IsBind,
		[Form] = @Form,
		[FormType] = @FormType
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfileUpdate */

/* Start DAM_CompositionProfileDelete */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileDelete]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileDelete];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileDelete]
	@Id int = null
AS
BEGIN
	DELETE [DAM_CompositionProfile]
	WHERE [Id] = @Id
END
GO
/* End DAM_CompositionProfileDelete */

/* Start DAM_CompositionProfileDeleteAll */
IF EXISTS
(
	SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[DAM_CompositionProfileDeleteAll]') AND type IN ( N'P', N'PC' )
)
DROP PROCEDURE [DAM_CompositionProfileDeleteAll];
GO
CREATE PROCEDURE [dbo].[DAM_CompositionProfileDeleteAll]
AS
BEGIN
	DELETE [DAM_CompositionProfile]
END
GO
/* End DAM_CompositionProfileDeleteAll */
