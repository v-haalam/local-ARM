// <copyright file="IPrepareToSendQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.PrepareToSendQueue
{
    /// <summary>
    /// interface for Prepare to send Queue.
    /// </summary>
    public interface IPrepareToSendQueue : IBaseQueue<PrepareToSendQueueMessageContent>
    {
    }
}
