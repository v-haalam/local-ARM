// <copyright file="DataQueueMessageOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Data.Func
{
    /// <summary>
    /// Options for data queue messages.
    /// </summary>
    public class DataQueueMessageOptions
    {
        /// <summary>
        /// Gets or sets the value for the delay to be applied to the
        /// requeued data queue trigger message if the trigger message
        /// is to be requeued in the first ten minutes of the notification
        /// being sent - this way, the initial results can be aggregated
        /// more often and displayed to the user faster so they are
        /// confident sending has started and is progressing.
        /// </summary>
        public double FirstTenMinutesRequeueMessageDelayInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the value for the delay to be applied to the
        /// requeued data queue trigger message if the trigger message
        /// is to be requeued after the first ten minutes of the notification
        /// being sent.
        /// </summary>
        public double RequeueMessageDelayInSeconds { get; set; }
    }
}
