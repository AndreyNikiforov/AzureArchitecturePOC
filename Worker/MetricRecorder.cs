using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class MetricRecorder : IMetricRecorder
    {
        private readonly CloudTableClient _client;
        private readonly string _tableName;

        public MetricRecorder(string connectionString, string tableName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            _client = account.CreateCloudTableClient();
            _tableName = tableName;
        }

        public void Reset()
        {
            _client.DeleteTableIfExist(_tableName);
            while (true)
                try
                {
                    _client.CreateTableIfNotExist(_tableName);
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

        public void Report(string action, string key, TimeSpan elapsed)
        {
            var context = _client.GetDataServiceContext();
            var entity = new MetricEntity(action, key, elapsed);
            context.AddObject(_tableName, entity);
            context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate);
        }
        public class MetricEntity : TableServiceEntity
        {
            public MetricEntity(string partition, string key, TimeSpan elapsed)
            {
                PartitionKey = partition;
                RowKey = key;
                ElapsedMilliseconds = elapsed.TotalMilliseconds;
            }

            public double ElapsedMilliseconds { get; set; }
        }
    }
}
