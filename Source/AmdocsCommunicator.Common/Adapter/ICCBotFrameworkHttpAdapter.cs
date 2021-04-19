﻿// <copyright file="ICCBotFrameworkHttpAdapter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Adapter
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// Bot Framework Http Adapter interface.
    /// </summary>
    public interface ICCBotFrameworkHttpAdapter : IAdapterIntegration
    {
        /// <summary>
        /// Creates a conversation on the specified channel.
        /// </summary>
        /// <param name="channelId">The ID for the channel.</param>
        /// <param name="serviceUrl">The channel's service URL endpoint.</param>
        /// <param name="credentials">The application credentials for the bot.</param>
        /// <param name="conversationParameters">The conversation information to use to create the conversation.</param>
        /// <param name="callback">The method to call for the resulting bot turn.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task CreateConversationAsync(string channelId, string serviceUrl, MicrosoftAppCredentials credentials, ConversationParameters conversationParameters, BotCallbackHandler callback, CancellationToken cancellationToken);
    }
}
