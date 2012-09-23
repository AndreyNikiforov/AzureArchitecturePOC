using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class DataEntity : TableServiceEntity
    {
        public DataEntity(int partition, int id, string loremIpsum)
        {
            this.PartitionKey = partition.ToString();
            this.RowKey = id.ToString();
            LoremIpsum = loremIpsum;
        }

        public DataEntity() { }

        public string LoremIpsum { get; set; }
    }
}