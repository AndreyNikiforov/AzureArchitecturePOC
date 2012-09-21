using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class CloudPopulator
    {
        private readonly CloudTableClient _client;
        private const string TableName = "dataloads";

        public CloudPopulator()
        {
            //create table if needed

            var connectionString = CloudConfigurationManager.GetSetting("CloudStorage.ConnectionString");
            var account = CloudStorageAccount.Parse(connectionString);
            _client = account.CreateCloudTableClient();
            _client.CreateTableIfNotExist(TableName);

        }

        public TimeSpan Run(int partition, int batch)
        {
            var stopWatch = Stopwatch.StartNew();
            var context = _client.GetDataServiceContext();
            for (var subBatch = 0; subBatch < 10; subBatch++)
            {
                for (var id = 0; id < 100; id++)
                {
                    var entity = new DataEntity(partition, partition*10000 + batch*1000 + subBatch*100 + id, 1000);
                    context.AddObject(TableName, entity);
                }
                context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate | SaveChangesOptions.Batch);
            }
            return stopWatch.Elapsed;
        }

    }
}
