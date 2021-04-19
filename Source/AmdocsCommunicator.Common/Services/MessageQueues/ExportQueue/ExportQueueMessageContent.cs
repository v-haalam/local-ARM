// <copyright file="ExportQueueMessageContent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.ExportQueue
{
    /// <summary>
    /// Azure service bus export queue message content class.
    /// </summary>
    public class ExportQueueMessageContent
    {
        /// <summary>
        /// Gets or sets the user id value.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the notification id value.
        /// </summary>
        public string NotificationId { get; set; }
    }
}
