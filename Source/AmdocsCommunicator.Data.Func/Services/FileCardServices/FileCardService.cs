// <copyright file="FileCardService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Data.Func.Services.FileCardServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.UserData;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Amdocs.Teams.App.Communicator.Common.Services.CommonBot;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Options;
    using Polly;

    /// <summary>
    /// The file card service to manage the card.
    /// </summary>
    public class FileCardService : IFileCardService
    {
        private readonly IUserDataRepository userDataRepository;
        private readonly string authorAppId;
        private readonly BotFrameworkHttpAdapter botAdapter;
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCardService"/> class.
        /// </summary>
        /// <param name="botOptions">the bot options.</param>
        /// <param name="botAdapter">the users service.</param>
        /// <param name="userDataRepository">the user data repository.</param>
        /// <param name="localizer">Localization service.</param>
        public FileCardService(
            IOptions<BotOptions> botOptions,
            BotFrameworkHttpAdapter botAdapter,
            IUserDataRepository userDataRepository,
            IStringLocalizer<Strings> localizer)
        {
            this.botAdapter = botAdapter ?? throw new ArgumentNullException(nameof(botAdapter));
            var options = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
            if (string.IsNullOrEmpty(options.Value?.AuthorAppId))
            {
                throw new ArgumentException("AuthorAppId setting is missing in the configuration.");
            }

            this.authorAppId = options.Value.AuthorAppId;
            this.userDataRepository = userDataRepository ?? throw new ArgumentNullException(nameof(userDataRepository));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Delete the card and send the permission expired message.
        /// </summary>
        /// <param name="userId">the user id.</param>
        /// <param name="fileConsentId">the file consent id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string userId, string fileConsentId)
        {
            var user = await this.userDataRepository.GetAsync(UserDataTableNames.UserDataPartition, userId);
            var conversationReference = new ConversationReference
            {
                ServiceUrl = user.ServiceUrl,
                Conversation = new ConversationAccount
                {
                    Id = user.ConversationId,
                },
            };
            string deleteText = this.localizer.GetString("FileCardExpireText");

            int maxNumberOfAttempts = 10;
            await this.botAdapter.ContinueConversationAsync(
               botAppId: this.authorAppId,
               reference: conversationReference,
               callback: async (turnContext, cancellationToken) =>
               {
                   // Retry it in addition to the original call.
                   var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(maxNumberOfAttempts, p => TimeSpan.FromSeconds(p));
                   await retryPolicy.ExecuteAsync(async () =>
                   {
                       await turnContext.DeleteActivityAsync(fileConsentId, cancellationToken);
                       var deleteMessage = MessageFactory.Text(deleteText);
                       deleteMessage.TextFormat = "xml";
                       await turnContext.SendActivityAsync(deleteMessage, cancellationToken);
                   });
               },
               cancellationToken: CancellationToken.None);
        }
    }
}
