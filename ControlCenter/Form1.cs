using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ControlCenter
{
    public partial class Form1 : Form
    {

        // The name of your queue (has to be lowercase)
        const string QueueName = Settings.ControlQueueName;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnPopulateSql_Click(object sender, EventArgs e)
        {
            btnPopulateSql.Enabled = false;
            //populates sql and cloud storages
            var connectionString = ConfigurationManager.AppSettings["CloudStore.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            //create multiple batches
            queue.AddMessage(
                new CloudQueueMessage(
                    MessageBase.Serialize(
                        new PopulateSqlMessage()
                        ))
                );
            btnPopulateSql.Enabled = true;
        }

        private void btnClearQueue_Click(object sender, EventArgs e)
        {
            btnClearQueue.Enabled = false;
            //cleans the queue
            var connectionString = ConfigurationManager.AppSettings["CloudStore.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();
            queue.Clear();
            btnClearQueue.Enabled = true;
        }

        private void btnPopulateCloud_Click(object sender, EventArgs e)
        {
            btnPopulateCloud.Enabled = false;

            //send a message to update data with LoremIpsum and create PK
            var connectionString = ConfigurationManager.AppSettings["CloudStore.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            for (var partition = 0; partition < 1000; partition++)
                queue.AddMessage(
                new CloudQueueMessage(
                    MessageBase.Serialize(
                        new PopulateCloudMessage()
                            {
                                Partition = partition
                            }))
                );

            btnPopulateCloud.Enabled = true;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            var connectionString = ConfigurationManager.AppSettings["CloudStore.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            queue.AddMessage(
                new CloudQueueMessage(
                    MessageBase.Serialize(
                        new CleanMessage()))
                );
            btnReset.Enabled = true;
        }

        private void btnRefreshCounts_Click(object sender, EventArgs e)
        {
            btnRefreshCounts.Enabled = false;
            var connectionString = ConfigurationManager.AppSettings["CloudStore.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var context = client.GetDataServiceContext();
            txtPopulateSql.Text = context.CreateQuery<MetricEntity>(Settings.MetricTableName).Where(entity => entity.PartitionKey == "PopulateSql").AsTableServiceQuery().Execute().Count().ToString();
            txtPopulateSqlLoremIpsum.Text = context.CreateQuery<MetricEntity>(Settings.MetricTableName).Where(entity => entity.PartitionKey == "PopulateSqlLoremIpsum").AsTableServiceQuery().Execute().Count().ToString();
            txtPopulateCloud.Text = context.CreateQuery<MetricEntity>(Settings.MetricTableName).Where(entity => entity.PartitionKey == "PopulateCloud").AsTableServiceQuery().Execute().Count().ToString();
            btnRefreshCounts.Enabled = true;

        }
    }
}
