// <copyright file="IExportQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.ExportQueue
{
    /// <summary>
    /// interface for Export Queue.
    /// </summary>
    public interface IExportQueue : IBaseQueue<ExportQueueMessageContent>
    {
    }
}
