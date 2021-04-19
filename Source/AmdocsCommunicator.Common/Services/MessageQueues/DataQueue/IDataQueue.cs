// <copyright file="IDataQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.DataQueue
{
    /// <summary>
    /// interface for DataQueue.
    /// </summary>
    public interface IDataQueue : IBaseQueue<DataQueueMessageContent>
    {
    }
}
