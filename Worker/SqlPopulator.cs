using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public SqlPopulator() : this(new SqlStorageContext(CloudConfigurationManager.GetSetting("SqlStore.ConnectionString")))
        {
        }

        /// <summary>
        /// Start populating storage with batch of data
        /// </summary>
        /// <param name="startFrom">id to start from</param>
        public TimeSpan Run(int startFrom = 0)
        {
            var stopWatch = Stopwatch.StartNew();
            _context.Database.ExecuteSqlCommand(
@"
insert INTO DataLoads 
(
	Id
)
(select id + @startFrom as num from Ids);	
", new SqlParameter("@startFrom", startFrom));
            return stopWatch.Elapsed;
        }
        public TimeSpan PopulateLoremIpsum(int loremIpsumBlobSize = 0)
        {
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
            var stopWatch = Stopwatch.StartNew();
            _context.Database.ExecuteSqlCommand(
@"
Update DataLoads 
	LoremIpsum = @loremIpsum;	
", new SqlParameter("@loremIpsum", loremIpsum));
            return stopWatch.Elapsed;
        }
        public TimeSpan BuildPK(int loremIpsumBlobSize = 0)
        {
            var stopWatch = Stopwatch.StartNew();
            _context.Database.ExecuteSqlCommand(
@"ALTER TABLE DataLoads ADD CONSTRAINT [PK_DataLoads] PRIMARY KEY NONCLUSTERED ([Id] ASC);");
            return stopWatch.Elapsed;
        }
    }
}
