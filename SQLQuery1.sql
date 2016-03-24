USE [Project]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[sgets]
		@studentid = N'13BD1A0502'

SELECT	@return_value as 'Return Value'

GO
