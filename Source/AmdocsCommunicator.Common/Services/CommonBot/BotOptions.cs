﻿// <copyright file="BotOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Common.Services.CommonBot
{
    /// <summary>
    /// Options used for holding metadata for the bot.
    /// </summary>
    public class BotOptions
    {
        /// <summary>
        /// Gets or sets the Microsoft app ID for the user bot.
        /// </summary>
        public string UserAppId { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft app password for the user bot.
        /// </summary>
        public string UserAppPassword { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft app ID for the author bot.
        /// </summary>
        public string AuthorAppId { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft app password for the author bot.
        /// </summary>
        public string AuthorAppPassword { get; set; }
    }
}
