// <copyright file="Startup.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(
    typeof(Amdocs.Teams.App.Communicator.Send.Func.Startup))]

namespace Amdocs.Teams.App.Communicator.Send.Func
{
    using System;
    using System.Globalization;
    using Amdocs.Teams.App.Communicator.Common.Repositories;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Common.Repositories.SentNotificationData;
    using Amdocs.Teams.App.Communicator.Common.Services.CommonBot;
    using Amdocs.Teams.App.Communicator.Common.Services.MessageQueues;
    using Amdocs.Teams.App.Communicator.Common.Services.MessageQueues.SendQueue;
    using Amdocs.Teams.App.Communicator.Common.Services.Teams;
    using Amdocs.Teams.App.Communicator.Send.Func.Services;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Register services in DI container of the Azure functions system.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add all options set from configuration values.
            builder.Services.AddOptions<SendFunctionOptions>()
                .Configure<IConfiguration>((sendFunctionOptions, configuration) =>
                {
                    sendFunctionOptions.MaxNumberOfAttempts =
                        configuration.GetValue<int>("MaxNumberOfAttempts", 1);

                    sendFunctionOptions.SendRetryDelayNumberOfSeconds =
                        configuration.GetValue<double>("SendRetryDelayNumberOfSeconds", 660);
                });
            builder.Services.AddOptions<BotOptions>()
                .Configure<IConfiguration>((botOptions, configuration) =>
                {
                    botOptions.UserAppId =
                        configuration.GetValue<string>("UserAppId");

                    botOptions.UserAppPassword =
                        configuration.GetValue<string>("UserAppPassword");
                });
            builder.Services.AddOptions<RepositoryOptions>()
                .Configure<IConfiguration>((repositoryOptions, configuration) =>
                {
                    repositoryOptions.StorageAccountConnectionString =
                        configuration.GetValue<string>("StorageAccountConnectionString");

                    // Defaulting this value to true because the main app should ensure all
                    // tables exist. It is here as a possible configuration setting in
                    // case it needs to be set differently.
                    repositoryOptions.EnsureTableExists =
                        !configuration.GetValue<bool>("IsItExpectedThatTableAlreadyExists", true);
                });
            builder.Services.AddOptions<MessageQueueOptions>()
                .Configure<IConfiguration>((messageQueueOptions, configuration) =>
                {
                    messageQueueOptions.ServiceBusConnection =
                        configuration.GetValue<string>("ServiceBusConnection");
                });

            builder.Services.AddLocalization();

            // Set current culture.
            var culture = Environment.GetEnvironmentVariable("i18n:DefaultCulture");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

            // Add bot services.
            builder.Services.AddSingleton<UserAppCredentials>();
            builder.Services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            builder.Services.AddSingleton<BotFrameworkHttpAdapter>();

            // Add teams services.
            builder.Services.AddTransient<IMessageService, MessageService>();

            // Add repositories.
            builder.Services.AddSingleton<ISendingNotificationDataRepository, SendingNotificationDataRepository>();
            builder.Services.AddSingleton<IGlobalSendingNotificationDataRepository, GlobalSendingNotificationDataRepository>();
            builder.Services.AddSingleton<ISentNotificationDataRepository, SentNotificationDataRepository>();

            // Add service bus message queues.
            builder.Services.AddSingleton<ISendQueue, SendQueue>();

            // Add the Notification service.
            builder.Services.AddTransient<INotificationService, NotificationService>();
        }
    }
}
