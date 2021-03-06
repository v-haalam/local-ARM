// <copyright file="TeamDataMap.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.Export.Mappers
{
    using System;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Amdocs.Teams.App.Communicator.Prep.Func.Export.Model;
    using CsvHelper.Configuration;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// Mapper class for TeamData.
    /// </summary>
    public sealed class TeamDataMap : ClassMap<TeamData>
    {
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamDataMap"/> class.
        /// </summary>
        /// <param name="localizer">Localization service.</param>
        public TeamDataMap(IStringLocalizer<Strings> localizer)
        {
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.Map(x => x.Id).Name(this.localizer.GetString("ColumnName_TeamId"));
            this.Map(x => x.Name).Name(this.localizer.GetString("ColumnName_TeamName"));
            this.Map(x => x.DeliveryStatus).Name(this.localizer.GetString("ColumnName_DeliveryStatus"));
            this.Map(x => x.StatusReason).Name(this.localizer.GetString("ColumnName_StatusReason"));
        }
    }
}
