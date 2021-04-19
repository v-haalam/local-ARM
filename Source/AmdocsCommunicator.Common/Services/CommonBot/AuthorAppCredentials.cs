﻿// <copyright file="AuthorAppCredentials.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.CommonBot
{
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// An author Microsoft app credentials object.
    /// </summary>
    public class AuthorAppCredentials : MicrosoftAppCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorAppCredentials"/> class.
        /// </summary>
        /// <param name="botOptions">The bot options.</param>
        public AuthorAppCredentials(IOptions<BotOptions> botOptions)
            : base(
                  appId: botOptions.Value.AuthorAppId,
                  password: botOptions.Value.AuthorAppPassword)
        {
        }
    }
}
