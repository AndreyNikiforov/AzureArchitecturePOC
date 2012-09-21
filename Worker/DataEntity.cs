using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class DataEntity : TableServiceEntity
    {
        public DataEntity(int partition, int id, int loremIpsumBlobSize)
        {
            this.PartitionKey = partition.ToString();
            this.RowKey = id.ToString();
            LoremIpsum =
                Enumerable.Repeat("Blah", loremIpsumBlobSize / 4)
                    .Aggregate(
                        new StringBuilder(),
                        (sb, s) =>
                            {
                                sb.Append(s);
                                return sb;
                            }, sb => sb.ToString());
        }

        public DataEntity() { }

        public string LoremIpsum { get; set; }
    }
}