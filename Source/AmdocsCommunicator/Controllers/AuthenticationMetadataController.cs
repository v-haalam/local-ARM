// <copyright file="AuthenticationMetadataController.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Amdocs.Teams.App.Communicator.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Controller for the authentication sign in data.
    /// </summary>
    [Route("api/authenticationMetadata")]
    public class AuthenticationMetadataController : ControllerBase
    {
        private readonly string tenantId;
        private readonly string clientId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMetadataController"/> class.
        /// </summary>
        /// <param name="authenticationOptions">The authentication options.</param>
        public AuthenticationMetadataController(IOptions<AuthenticationOptions> authenticationOptions)
        {
            if (authenticationOptions is null)
            {
                throw new ArgumentNullException(nameof(authenticationOptions));
            }

            this.tenantId = authenticationOptions.Value.AzureAdTenantId;
            this.clientId = authenticationOptions.Value.AzureAdClientId;
        }

        /// <summary>
        /// Get authentication consent Url.
        /// </summary>
        /// <param name="windowLocationOriginDomain">Window location origin domain.</param>
        /// <param name="loginHint">UPN value.</param>
        /// <returns>Conset Url.</returns>
        [HttpGet("consentUrl")]
        public string GetConsentUrl(
            [FromQuery]string windowLocationOriginDomain,
            [FromQuery]string loginHint)
        {
            if (windowLocationOriginDomain == null)
            {
                throw new ArgumentNullException(nameof(windowLocationOriginDomain));
            }

            if (loginHint == null)
            {
                throw new ArgumentNullException(nameof(loginHint));
            }

            var consentUrlComponentDictionary = new Dictionary<string, string>
            {
                ["redirect_uri"] = $"https://{windowLocationOriginDomain}/signin-simple-end",
                ["client_id"] = this.clientId,
                ["response_type"] = "id_token",
                ["response_mode"] = "fragment",
                ["scope"] = "https://graph.microsoft.com/User.Read openid profile",
                ["nonce"] = Guid.NewGuid().ToString(),
                ["state"] = Guid.NewGuid().ToString(),
                ["login_hint"] = loginHint,
            };
            var consentUrlComponentList = consentUrlComponentDictionary
                .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}")
                .ToList();

            var consentUrlPrefix = $"https://login.microsoftonline.com/{this.tenantId}/oauth2/v2.0/authorize?";

            var consentUrlString = consentUrlPrefix + string.Join('&', consentUrlComponentList);

            return consentUrlString;
        }
    }
}
