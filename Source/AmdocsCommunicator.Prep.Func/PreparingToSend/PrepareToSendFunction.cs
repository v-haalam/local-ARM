// <copyright file="PrepareToSendFunction.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func
{
    using System;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.PrepareToSendQueue;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure Function App triggered by messages from a Service Bus queue. <see cref="PrepareToSendQueue.QueueName"/>
    ///
    /// The function processes data from service bus queue and prepares the data to be processed in send, data queue.
    /// </summary>
    public class PrepareToSendFunction
    {
        private readonly INotificationDataRepository notificationDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareToSendFunction"/> class.
        /// </summary>
        /// <param name="notificationDataRepository">Notification data repository.</param>
        public PrepareToSendFunction(
            INotificationDataRepository notificationDataRepository)
        {
            this.notificationDataRepository = notificationDataRepository ?? throw new ArgumentNullException(nameof(notificationDataRepository));
        }

        /// <summary>
        /// Azure Function App triggered by messages from a Service Bus queue.
        /// It kicks off the durable orchestration for preparing to send notifications.
        /// </summary>
        /// <param name="myQueueItem">The Service Bus queue item.</param>
        /// <param name="starter">Durable orchestration client.</param>
        /// <param name="log">Logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(FunctionNames.PrepareToSendFunction)]
        public async Task Run(
            [ServiceBusTrigger(PrepareToSendQueue.QueueName, Connection = PrepareToSendQueue.ServiceBusConnectionConfigurationKey)]
            string myQueueItem,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Get Notification Data
            var queueMessageContent = JsonConvert.DeserializeObject<PrepareToSendQueueMessageContent>(myQueueItem);
            var notificationId = queueMessageContent.NotificationId;
            var sentNotificationDataEntity = await this.notificationDataRepository.GetAsync(
                partitionKey: NotificationDataTableNames.SentNotificationsPartition,
                rowKey: notificationId);

            if (sentNotificationDataEntity == null)
            {
                log.LogError($"Notification entity not found. Notification Id: {notificationId}");
                return;
            }

            // Start PrepareToSendOrchestrator function.
            string instanceId = await starter.StartNewAsync(
                FunctionNames.PrepareToSendOrchestrator,
                sentNotificationDataEntity);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}
