using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class CloudPopulator
    {
        private readonly CloudTableClient _client;
        private const string TableName = "LoremIpsum";

        private readonly IMetricRecorder _recorder;

        public CloudPopulator()
            : this(CloudConfigurationManager.GetSetting("CloudStore.ConnectionString"), new MetricRecorder(CloudConfigurationManager.GetSetting("CloudStore.ConnectionString"), Settings.MetricTableName))
        {
        }

        public CloudPopulator(string connectionString, IMetricRecorder recorder)
        {
            //create table if needed

            var account = CloudStorageAccount.Parse(connectionString);
            _client = account.CreateCloudTableClient();
            _client.CreateTableIfNotExist(TableName);

            _recorder = recorder;

        }

        public TimeSpan Populate(CancellationToken cancel, int partition)
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
            var context = _client.GetDataServiceContext();
            var elapsed = TimeSpan.Zero;
            for (var batch = 0; batch < 100 && !cancel.IsCancellationRequested; batch++)
            {
                var stopWatch = Stopwatch.StartNew();
                for (var id = 0; id < 100 && !cancel.IsCancellationRequested; id++)
                {
                    var entity = new DataEntity(partition, partition*1000 + batch*100 + id,
                                                loremIpsum);
                    context.AddObject(TableName, entity);
                }
                context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.Batch);
                stopWatch.Stop();
                elapsed += stopWatch.Elapsed;
            }
            _recorder.Report("PopulateCloud", partition.ToString(), elapsed);
            return elapsed;
        }

        public void Initialize()
        {
            _client.DeleteTableIfExist(TableName);
            while (true)
                try
                {
                    _client.CreateTableIfNotExist(TableName);
                    break;
                }
                catch (StorageClientException e)
                {
                    //deleting make take 40sec (GC)
                    if (e.StatusCode != HttpStatusCode.Conflict)
                        throw;
                    Thread.Sleep(1000);
                }
        }
    }
}
