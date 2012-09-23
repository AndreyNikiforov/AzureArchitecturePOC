using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Microsoft.WindowsAzure;
using Worker.SqlData;

namespace Worker
{
    /// <summary>
    /// This class is responsible for populating sql storage with data for testing
    /// </summary>
    public class SqlPopulator
    {
        private readonly SqlStorageContext _context;
        private readonly IMetricRecorder _recorder;

        public SqlPopulator(SqlStorageContext context, IMetricRecorder recorder)
        {
            _context = context;
            _recorder = recorder;
        }

        /// <summary>
        /// ctor to use when DI is not available
        /// </summary>
        public SqlPopulator()
            : this(new SqlStorageContext(CloudConfigurationManager.GetSetting("SqlStore.ConnectionString")), new MetricRecorder(CloudConfigurationManager.GetSetting("CloudStore.ConnectionString"), Settings.MetricTableName))
        {
        }

        /// <summary>
        /// Start populating storage with batch of data
        /// </summary>
        /// <param name="cancel">if has to be cancelled</param>
        public void Popluate(CancellationToken cancel)
        {
            for (var batch = 0; batch < 100 && !cancel.IsCancellationRequested; batch++)
            {
                var stopWatch = Stopwatch.StartNew();
                _context.Database.ExecuteSqlCommand(
                    @"
MERGE INTO LoremIpsum Target
USING (select id + @startFrom as num from ids) Source
ON Target.id = Source.Num
WHEN NOT MATCHED THEN INSERT (
	Id
) VALUES (
	source.num
);
", new SqlParameter("@startFrom", batch*100000));
                stopWatch.Stop();

                //record metrics

                _recorder.Report("PopulateSql", batch.ToString(), stopWatch.Elapsed);
            }
        }
        public void PopulateLoremIpsum(CancellationToken cancel)
        {
            const int loremIpsumBlobSize = 1000;
            //gerenerate lorem ipsum text
            var loremIpsum =
                Enumerable.Repeat("Blah", loremIpsumBlobSize/4)
                    .Aggregate(
                        new StringBuilder(),
                        (sb, s) =>
                            {
                                sb.Append(s);
                                return sb;
                            }, sb => sb.ToString());
            for (var batch = 0; batch < 10000 && !cancel.IsCancellationRequested; batch++)
            {
                var stopWatch = Stopwatch.StartNew();
                _context.Database.ExecuteSqlCommand(
                    @"
Update LoremIpsum 
SET LoremIpsum = @loremIpsum
where id in (select id from LoremIpsum with (nolock) where id between @startFrom and @startFrom + 10000);
", new SqlParameter("@loremIpsum", loremIpsum), new SqlParameter("@startFrom", batch * 10000));
                stopWatch.Stop();

                //record metrics

                _recorder.Report("PopulateSqlLoremIpsum", batch.ToString(), stopWatch.Elapsed);
            }

        }

        /// <summary>
        /// Rebuilds SQL data, and metrics tables
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            var stopWatch = Stopwatch.StartNew();
            _context.Database.ExecuteSqlCommand(
                @"
if exists(select * from information_schema.tables where table_name = 'LoremIpsum')
    drop table LoremIpsum;
");
            _context.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[LoremIpsum](
	[Id] [int] NOT NULL,
	--We need CLUSTERED index for Azure; IDENTITY column seems to be the fastest approach
	--Cannot use PK as CLUSTERED, beacuse batches may be added at random order
	[Stamp] [INT] NOT NULL IDENTITY,
	[LoremIpsum] [nvarchar](max) NULL,
CONSTRAINT [PK_LoremIpsum] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
CREATE CLUSTERED INDEX IX_LoremIpsum_Stamp on LoremIpsum ([Stamp]);
");
            _context.Database.ExecuteSqlCommand(
@"if exists(select * from information_schema.tables where table_name = 'IDs')
    drop table Ids;
");
            _context.Database.ExecuteSqlCommand(
@"CREATE TABLE IDs (
	Id INT NOT NULL,
	CONSTRAINT PK_Ids PRIMARY KEY CLUSTERED (ID ASC)
);
");
            _context.Database.ExecuteSqlCommand(
@"with 
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
");
            _recorder.Report("InitializeSql", string.Empty, stopWatch.Elapsed);
        }
    }
}
