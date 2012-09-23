using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
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
            _client.CreateTable(_tableName);
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
