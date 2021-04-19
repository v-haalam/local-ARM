// <copyright file="MetadataMap.cs" company="Microsoft">
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
    /// Mapper class for MetaData.
    /// </summary>
    public sealed class MetadataMap : ClassMap<Metadata>
    {
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataMap"/> class.
        /// </summary>
        /// <param name="localizer">Localization service.</param>
        public MetadataMap(IStringLocalizer<Strings> localizer)
        {
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.Map(x => x.MessageTitle).Name(this.localizer.GetString("ColumnName_MessageTitle"));
            this.Map(x => x.SentTimeStamp).Name(this.localizer.GetString("ColumnName_SentTimeStamp"));
            this.Map(x => x.ExportTimeStamp).Name(this.localizer.GetString("ColumnName_ExportTimeStamp"));
            this.Map(x => x.ExportedBy).Name(this.localizer.GetString("ColumnName_ExportedBy"));
        }
    }
}
