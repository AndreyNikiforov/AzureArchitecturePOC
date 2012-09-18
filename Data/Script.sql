--DROP TABLE [DataLoad]

--Create table
CREATE TABLE [dbo].[DataLoads](
	[Id] [int] NOT NULL,
	[Stamp] [DateTime] NOT NULL DEFAULT GetDate(),
	[LoremIpsum] [nvarchar](max) NULL,
 CONSTRAINT [PK_DataLoads] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
CREATE CLUSTERED INDEX IX_Stamp on DataLoads ([Stamp]);
GO

