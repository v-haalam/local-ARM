// <copyright file="GetRecipientsActivity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Common.Repositories.SentNotificationData;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// Reads all the recipients from Sent notification table.
    /// </summary>
    public class GetRecipientsActivity
    {
        private readonly ISentNotificationDataRepository sentNotificationDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRecipientsActivity"/> class.
        /// </summary>
        /// <param name="sentNotificationDataRepository">The sent notification data repository.</param>
        public GetRecipientsActivity(ISentNotificationDataRepository sentNotificationDataRepository)
        {
            this.sentNotificationDataRepository = sentNotificationDataRepository ?? throw new ArgumentNullException(nameof(sentNotificationDataRepository));
        }

        /// <summary>
        /// Reads all the recipients from Sent notification table.
        /// </summary>
        /// <param name="notification">notification.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(FunctionNames.GetRecipientsActivity)]
        public async Task<IEnumerable<SentNotificationDataEntity>> GetRecipientsAsync([ActivityTrigger] NotificationDataEntity notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var recipients = await this.sentNotificationDataRepository.GetAllAsync(notification.Id);
            return recipients;
        }

        /// <summary>
        /// Reads all the recipients from Sent notification table who do not have conversation details.
        /// </summary>
        /// <param name="notification">notification.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [FunctionName(FunctionNames.GetPendingRecipientsActivity)]
        public async Task<IEnumerable<SentNotificationDataEntity>> GetPendingRecipientsAsync([ActivityTrigger] NotificationDataEntity notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var recipients = await this.sentNotificationDataRepository.GetAllAsync(notification.Id);
            return recipients.Where(recipient => string.IsNullOrEmpty(recipient.ConversationId));
        }
    }
}
