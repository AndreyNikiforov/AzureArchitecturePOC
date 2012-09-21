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
        const string QueueName = "controlqueue";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            //populates sql and cloud storages
            var connectionString = ConfigurationManager.AppSettings["CloudStorage.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            //create multiple batches
            for(var batch = 0; batch <100; batch++)
            {
                queue.AddMessage(
                    new CloudQueueMessage(
                        MessageBase.Serialize(
                            new PopulateSqlMessage()
                                {
                                    StartFrom = batch * 100000 //each batch is a hundred thousands
                                }))
                    );
            }
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            //cleans the queue
            var connectionString = ConfigurationManager.AppSettings["CloudStorage.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();
            queue.Clear();
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            //send a message to update data with LoremIpsum and create PK
            var connectionString = ConfigurationManager.AppSettings["CloudStorage.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            //create multiple batches
            for (var batch = 0; batch < 1000; batch++)
                queue.AddMessage(
                    new CloudQueueMessage(
                        MessageBase.Serialize(
                            new PopulateSqlLoremIpsumMessage()
                                {
                                    LoremIpsumBlobSize = 1000,
                                    StartFrom = batch*10000 //each batch is a hundred thousands
                                }))
                    );

            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;

            //send a message to update data with LoremIpsum and create PK
            var connectionString = ConfigurationManager.AppSettings["CloudStorage.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            queue.AddMessage(
                new CloudQueueMessage(
                    MessageBase.Serialize(
                        new PopulateSqlBuildPKMessage()
                        ))
                );

            button4.Enabled = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;

            //send a message to update data with LoremIpsum and create PK
            var connectionString = ConfigurationManager.AppSettings["CloudStorage.ConnectionString"];
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueName);
            queue.CreateIfNotExist();

            //create multiple batches
            //break data into 1000 partition, each will be populated in 10 batches
            for (var partition = 0; partition < 1000; partition++)
                for (var batch = 0; batch < 10; batch++)
                    queue.AddMessage(
                    new CloudQueueMessage(
                        MessageBase.Serialize(
                            new PopulateCloudMessage()
                            {
                                Partition = partition,
                                Batch = batch
                            }))
                    );

            button5.Enabled = true;

        }
    }
}
