using System;
using System.Collections.Generic;
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

namespace Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue (has to be lowercase)
        const string QueueName = "controlqueue";

        CloudQueue _queue;
        bool _isStopped;

        public override void Run()
        {
            while (!_isStopped)
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
                        if (msg is PopulateSqlMessage)
                            new SqlPopulator().Run(((PopulateSqlMessage)msg).StartFrom);

                        _queue.DeleteMessage(receivedMessage);
                    }
                }
                catch (StorageException e)
                {
                    if (e.ExtendedErrorInformation != null 
                        && !e.ExtendedErrorInformation.ErrorCode.Equals(StorageErrorCodeStrings.InternalError))
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);    //pause and retry
                }
                catch (OperationCanceledException e)
                {
                    if (!_isStopped)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections  -- do we need this??
            //ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            var connectionString = CloudConfigurationManager.GetSetting("CloudStorage.ConnectionString");
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
            
            _isStopped = false;
            return base.OnStart();
        }

        public override void OnStop()
        {
            _isStopped = true;
            base.OnStop();
        }

    }
}
