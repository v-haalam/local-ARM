// <copyright file="GetMetadataActivity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.Export.Activities
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.ExportData;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Amdocs.Teams.App.Communicator.Common.Services.MicrosoftGraph;
    using Amdocs.Teams.App.Communicator.Prep.Func.Export.Model;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Localization;
    using Microsoft.Graph;

    /// <summary>
    /// Activity to create the metadata.
    /// </summary>
    public class GetMetadataActivity
    {
        private readonly IUsersService usersService;
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMetadataActivity"/> class.
        /// </summary>
        /// <param name="usersService">the users service.</param>
        /// <param name="localizer">Localization service.</param>
        public GetMetadataActivity(
            IUsersService usersService,
            IStringLocalizer<Strings> localizer)
        {
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Create and get the metadata.
        /// </summary>
        /// <param name="exportRequiredData">Tuple containing notification data entity and export data entity.</param>
        /// <returns>instance of metadata.</returns>
        [FunctionName(FunctionNames.GetMetadataActivity)]
        public async Task<Metadata> GetMetadataActivityAsync(
            [ActivityTrigger](NotificationDataEntity notificationDataEntity,
            ExportDataEntity exportDataEntity) exportRequiredData)
        {
            if (exportRequiredData.notificationDataEntity == null)
            {
                throw new ArgumentNullException(nameof(exportRequiredData.notificationDataEntity));
            }

            if (exportRequiredData.exportDataEntity == null)
            {
                throw new ArgumentNullException(nameof(exportRequiredData.exportDataEntity));
            }

            User user = default;
            try
            {
                user = await this.usersService.GetUserAsync(exportRequiredData.exportDataEntity.PartitionKey);
            }
            catch (ServiceException serviceException)
            {
                if (serviceException.StatusCode != HttpStatusCode.Forbidden)
                {
                    throw serviceException;
                }
            }

            var userPrincipalName = (user != null) ?
                user.UserPrincipalName :
                this.localizer.GetString("AdminConsentError");

            return this.Create(
                exportRequiredData.notificationDataEntity,
                exportRequiredData.exportDataEntity,
                userPrincipalName);
        }

        private Metadata Create(
            NotificationDataEntity notificationDataEntity,
            ExportDataEntity exportDataEntity,
            string userPrinicipalName)
        {
            var metadata = new Metadata
            {
                MessageTitle = notificationDataEntity.Title,
                SentTimeStamp = notificationDataEntity.SentDate,
                ExportedBy = userPrinicipalName,
                ExportTimeStamp = exportDataEntity.SentDate,
            };
            return metadata;
        }
    }
}