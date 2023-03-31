-- noinspection SqlWithoutWhereForFile

IF NOT EXISTS (SELECT * FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[dbo].[FuelPrice]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[FuelPrice](
		[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
		[Period] [date] NOT NULL,
		[Price] [decimal](6,3) NOT NULL

		CONSTRAINT [PK_FuelPrice] PRIMARY KEY CLUSTERED ([Id] ASC)
		WITH (
			  PAD_INDEX = OFF
			, STATISTICS_NORECOMPUTE = OFF
			, IGNORE_DUP_KEY = OFF
			, ALLOW_ROW_LOCKS = ON
			, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END;
GO

IF TYPE_ID(N'FuelPrice') IS NULL
CREATE TYPE [dbo].[FuelPrice] AS TABLE (
	  [Period] [date] NOT NULL
	, [Price] [decimal](6,3) NOT NULL
)
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_InsertFuelPrice] (
	  @Prices AS [dbo].[FuelPrice] READONLY
	, @RetentionPeriodDays [int]
)
AS
BEGIN
	MERGE INTO [dbo].[FuelPrice] target
	USING @Prices AS source ON target.[Period] = source.[Period]
	WHEN NOT MATCHED BY TARGET
		AND DATEDIFF(DAY, source.[Period], GETDATE()) <= @RetentionPeriodDays
	THEN
	INSERT ([Period], [Price])
	VALUES (source.[Period], source.[Price]);
END
GO