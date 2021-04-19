// <copyright file="DraftNotificationSummary.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Models
{
    using System;

    /// <summary>
    /// Draft Notification Summary model class.
    /// </summary>
    public class DraftNotificationSummary
    {
        /// <summary>
        /// Gets or sets Notification Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Title value.
        /// </summary>
        public string Title { get; set; }
    }
}
