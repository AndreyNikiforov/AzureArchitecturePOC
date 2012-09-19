--DROP TABLE [DataLoads]

--Create table
CREATE TABLE [dbo].[DataLoads](
	[Id] [int] NOT NULL,
	--We need CLUSTERED index for Azure; IDENTITY column seems to be the fastest approach
	--Cannot use PK as CLUSTERED, beacuse batches may be added at random order
	[Stamp] [INT] NOT NULL IDENTITY,
	[LoremIpsum] [nvarchar](max) NULL
-- No PK yet. It will be created AFTER data is populated
);
CREATE CLUSTERED INDEX IX_Stamp on DataLoads ([Stamp]);
GO

--materializes list of ids -- faster than build it in every query
CREATE TABLE IDs (
	Id INT NOT NULL,
	CONSTRAINT PK_Ids PRIMARY KEY CLUSTERED (ID ASC)
);
GO 

--... and populate IDs with data
with 
digits as (
select 0 as digit
union all 
select 1 as digit
union all 
select 2 as digit
union all
select 3 as digit
union all
select 4 as digit
union all
select 5 as digit
union all
select 6 as digit
union all
select 7 as digit
union all
select 8 as digit
union all
select 9 as digit
),
thnd as (
select 
	d0.digit + d1.digit * 10 + d2.digit * 100 as num
from
	digits d0
	cross join digits d1
	cross join digits d2
),
mln as (
select t0.num + t1.num * 1000 as num
from
	thnd t0
	cross join thnd t1
),
hthnds as (
select t0.num + d0.digit * 1000 + d1.digit * 10000 as num
from
	thnd t0
	cross join digits d0
	cross join digits d1
)
insert into Ids (id)
(select num from hthnds);
GO
