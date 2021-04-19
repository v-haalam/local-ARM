// <copyright file="MustBeValidUpnRequirement.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Authentication
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This class is an authorization policy requirement.
    /// It specifies that an id token must contain Upn claim.
    /// </summary>
    public class MustBeValidUpnRequirement : IAuthorizationRequirement
    {
    }
}
