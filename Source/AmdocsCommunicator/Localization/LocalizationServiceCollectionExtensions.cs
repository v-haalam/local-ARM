// <copyright file="LocalizationServiceCollectionExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Localization
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension class for registering localization services in DI container.
    /// </summary>
    public static class LocalizationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds localization settings to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddLocalizationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var localeOptions = configuration.GetSection("i18n").Get<LocaleOptions>();

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var defaultCulture = CultureInfo.GetCultureInfo(localeOptions.DefaultCulture);
                var supportedCultures = localeOptions.SupportedCultures.Split(',')
                    .Select(culture => CultureInfo.GetCultureInfo(culture))
                    .ToList();

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new RequestCultureProvider(),
                };
            });
        }
    }
}
