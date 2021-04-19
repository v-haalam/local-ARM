// <copyright file="PrepareToSendQueueMessageContent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.PrepareToSendQueue
{
    /// <summary>
    /// Azure service bus prepare to send queue message content class.
    /// </summary>
    public class PrepareToSendQueueMessageContent
    {
        /// <summary>
        /// Gets or sets the notification id value.
        /// </summary>
        public string NotificationId { get; set; }
    }
}
