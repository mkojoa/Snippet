USE [PersolHCM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE spCodesSetup_GenericsCreation
@iCompanyId uniqueidentifier,
@szCode nvarchar(50),
@szDescription nvarchar(200),
@szType varchar(10),
@szNotes text,
@iStatus int,
@iUserId uniqueidentifier
AS
BEGIN
				DECLARE @pkId table(pkId uniqueidentifier);

				INSERT INTO [dbo].[psHCMCodes]
					   ([iCompanyId]
					   ,[szCode]
					   ,[szDescription]
					   ,[szType]
					   ,[szNotes]
					   ,[iStatus]
					   ,[dLastDateModified]
					   ,[iUserId]
					   ,[dCreatedate])
					OUTPUT inserted.pkId into @pkId(pkId)
				 VALUES
					   (@iCompanyId,
						@szCode,
						@szDescription,
						@szType,
						@szNotes,
						@iStatus,
						getdate(),
						@iUserId,
						getdate())

						   IF EXISTS(SELECT top 1 pkId FROM [psHCMCodes] where pkId = (SELECT pkId FROM @pkId))
							   BEGIN
									SELECT szCode [Code]
										  ,szDescription [Description]
										  ,szType [Type]
										  ,szNotes [Note]
										  ,iStatus [Status]
									  FROM [dbo].[psHCMCodes] WHERE pkId = (SELECT pkId FROM @pkId)
							   END
							ELSE
							   BEGIN
									SELECT @szCode [Code]
										  ,@szDescription [Description]
										  ,@szType [Type]
										  ,@szNotes [Note]
										  ,@iStatus [Status]
							   END
END