using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Common
{
    public class MetricEntity : TableServiceEntity
    {
        public MetricEntity()
        {
            
        }

        public MetricEntity(string partition, string key, TimeSpan elapsed)
        {
            PartitionKey = partition;
            RowKey = key;
            ElapsedMilliseconds = elapsed.TotalMilliseconds;
        }

        public double ElapsedMilliseconds { get; set; }
    }
}