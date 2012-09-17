using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public SqlPopulator(SqlStorageContext context)
        {
            _context = context;
        }

        /// <summary>
        /// ctor to use when DI is not available
        /// </summary>
        public SqlPopulator() : this(new SqlStorageContext(CloudConfigurationManager.GetSetting("SqlStore.ConnextionString")))
        {
        }

        /// <summary>
        /// Start populating storage with batch of data
        /// </summary>
        /// <param name="startFrom">id to start from</param>
        public TimeSpan Run(int startFrom = 0)
        {
            //gerenerate lorem ipsum text
            var loremIpsum =
                Enumerable.Repeat("Blah", 1000)
                    .Aggregate(
                        new StringBuilder(),
                        (sb, s) =>
                            {
                                sb.Append(s);
                                return sb;
                            }, sb => sb.ToString());
            var stopWatch = Stopwatch.StartNew();
            _context.Database.ExecuteSqlCommand(
@"
--populate data
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
)
MERGE INTO DataLoad Target
USING (select mln.num + @startFrom as num from tmln) Source
ON Target.id = Source.Num
WHEN NOT MATCHED THEN INSERT (
	Id
	, LoremIpsum
) VALUES (
	source.num
	, @LoremIpsum
);	
", startFrom, loremIpsum);
            return stopWatch.Elapsed;
        }
    }
}
