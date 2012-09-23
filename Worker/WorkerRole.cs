using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Worker.SqlData;

namespace Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue (has to be lowercase)
        const string QueueName = "controlqueue";

        CloudQueue _queue;
        private CancellationTokenSource _cancel = new CancellationTokenSource();

        public override void Run()
        {
            while (!_cancel.IsCancellationRequested)
            {
                try
                {
                    // Receive the message
                    var receivedMessage = _queue.GetMessage();

                    if (receivedMessage != null)
                    {
                        // Process the message

                        // Deserialize message
                        var msg = MessageBase.Deserialize(receivedMessage.AsString);

                        // simple selector
                        if (msg is CleanMessage)
                        {
                            var stopWatch = Stopwatch.StartNew();
                            new MetricRecorder(CloudConfigurationManager.GetSetting("CloudStore.ConnectionString"), "MetricTable").Reset();
                            new SqlPopulator().Initialize();
                            new CloudPopulator().Initialize();
                            Trace.TraceInformation("--------------- CleanMessage completed in {0}", stopWatch.Elapsed);
                        }
                        else if (msg is PopulateSqlMessage)
                        {
                            ThreadPool.QueueUserWorkItem(delegate { new SqlPopulator().Popluate(_cancel.Token); });
                            Trace.TraceInformation("--------------- PopulateSqlMessage requested");
                        }
                        else if (msg is PopulateSqlLoremIpsumMessage)
                        {
                            ThreadPool.QueueUserWorkItem(delegate { new SqlPopulator().PopulateLoremIpsum(_cancel.Token); });
                            Trace.TraceInformation("--------------- PopulateLoremIpsumMessage requested");
                        }
                        else if (msg is PopulateCloudMessage)
                        {
                            var typedMsg = (PopulateCloudMessage) msg;
                            var elapsed = new CloudPopulator().Populate(_cancel.Token, typedMsg.Partition);
                            Trace.TraceInformation("--------------- PopulateCloudMessage for partition {0} populated in {1}", typedMsg.Partition, elapsed);
                        }
                        else
                            Trace.TraceError("Received message of unsupported type {0}. Swallowing", msg.GetType().FullName);

                        _queue.DeleteMessage(receivedMessage);
                    }
                }
                catch (StorageException e)
                {
                    if (e.ExtendedErrorInformation != null 
                        && !e.ExtendedErrorInformation.ErrorCode.Equals(StorageErrorCodeStrings.InternalError))
                    {
                        Trace.TraceError(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);    //pause and retry
                }
                catch (OperationCanceledException e)
                {
                    if (!_cancel.IsCancellationRequested)
                    {
                        Trace.TraceError(e.Message);
                        throw;
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    throw;
                }
            }
        }

        public override bool OnStart()
        {
            //EF stuff -- do not initialize db
            Database.SetInitializer<SqlStorageContext>(null);

            // Set the maximum number of concurrent connections  -- do we need this??
            //ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            var connectionString = CloudConfigurationManager.GetSetting("CloudStore.ConnectionString");
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            _queue = client.GetQueueReference(QueueName);
            _queue.CreateIfNotExist();

            // set diagnostics
            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            // Windows Azure logs
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0);    //60 sec is min
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Undefined;    //Undefined == everything

            var diagAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

            var roleInstanceDiagnosticManager =
                diagAccount.CreateRoleInstanceDiagnosticManager(RoleEnvironment.DeploymentId,
                                                                   RoleEnvironment.CurrentRoleInstance.Role.Name,
                                                                   RoleEnvironment.CurrentRoleInstance.Id);
            roleInstanceDiagnosticManager.SetCurrentConfiguration(config);
            
            return base.OnStart();
        }

        public override void OnStop()
        {
            _cancel.Cancel();
            base.OnStop();
        }

    }
}
