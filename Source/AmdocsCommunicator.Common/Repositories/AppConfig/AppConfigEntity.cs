// <copyright file="AppConfigEntity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Repositories
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// App configuration entity.
    /// </summary>
    public class AppConfigEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the entity value.
        /// </summary>
        public string Value { get; set; }
    }
}
