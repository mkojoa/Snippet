USE [PersolHCM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE spCodes_CheckIfByCodeExist
@szCode nvarchar(50),
@uniCompanyId uniqueidentifier
AS
BEGIN
		IF EXISTS (SELECT 1 FROM psHCMCodes where szCode = @szCode AND iCompanyId = @uniCompanyId)
			BEGIN
					SELECT 1;
			END
		ELSE
			BEGIN
					SELECT 0;
			END
END		