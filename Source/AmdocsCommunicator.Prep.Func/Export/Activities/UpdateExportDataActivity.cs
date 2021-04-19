// <copyright file="UpdateExportDataActivity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.Export.Activities
{
    using System;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.ExportData;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    /// <summary>
    /// Activity to update export data.
    /// </summary>
    public class UpdateExportDataActivity
    {
        private readonly IExportDataRepository exportDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateExportDataActivity"/> class.
        /// </summary>
        /// <param name="exportDataRepository">the export data respository.</param>
        public UpdateExportDataActivity(IExportDataRepository exportDataRepository)
        {
            this.exportDataRepository = exportDataRepository ?? throw new ArgumentNullException(nameof(exportDataRepository));
        }

        /// <summary>
        /// Update the export data.
        /// </summary>
        /// <param name="exportDataEntity">Export data entity.</param>
        /// <returns>A <see cref="Task"/>Representing the asynchronous operation.</returns>
        [FunctionName(FunctionNames.UpdateExportDataActivity)]
        public async Task UpdateExportDataActivityAsync(
            [ActivityTrigger] ExportDataEntity exportDataEntity)
        {
            if (exportDataEntity == null)
            {
                throw new ArgumentNullException(nameof(exportDataEntity));
            }

            await this.exportDataRepository.CreateOrUpdateAsync(exportDataEntity);
        }
    }
}