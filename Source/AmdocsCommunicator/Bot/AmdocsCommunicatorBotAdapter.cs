// <copyright file="AmdocsCommunicatorBotAdapter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Bot
{
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;

    /// <summary>
    /// The Bot Adapter.
    /// </summary>
    public class AmdocsCommunicatorBotAdapter : BotFrameworkHttpAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmdocsCommunicatorBotAdapter"/> class.
        /// </summary>
        /// <param name="credentialProvider">Credential provider service instance.</param>
        /// <param name="botFilterMiddleware">Teams message filter middleware instance.</param>
        public AmdocsCommunicatorBotAdapter(
            ICredentialProvider credentialProvider,
            AmdocsCommunicatorBotFilterMiddleware botFilterMiddleware)
            : base(credentialProvider)
        {
            this.Use(botFilterMiddleware);
        }
    }
}
