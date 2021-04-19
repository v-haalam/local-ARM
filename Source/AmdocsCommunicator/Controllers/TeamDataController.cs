// <copyright file="TeamDataController.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Authentication;
    using Amdocs.Teams.App.Communicator.Common.Repositories.TeamData;
    using Amdocs.Teams.App.Communicator.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for the teams data.
    /// </summary>
    [Route("api/teamData")]
    [Authorize(PolicyNames.MustBeValidUpnPolicy)]
    public class TeamDataController : ControllerBase
    {
        private readonly ITeamDataRepository teamDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamDataController"/> class.
        /// </summary>
        /// <param name="teamDataRepository">Team data repository instance.</param>
        public TeamDataController(ITeamDataRepository teamDataRepository)
        {
            this.teamDataRepository = teamDataRepository ?? throw new ArgumentNullException(nameof(teamDataRepository));
        }

        /// <summary>
        /// Get data for all teams.
        /// </summary>
        /// <returns>A list of team data.</returns>
        [HttpGet]
        public async Task<IEnumerable<TeamData>> GetAllTeamDataAsync()
        {
            var entities = await this.teamDataRepository.GetAllSortedAlphabeticallyByNameAsync();
            var result = new List<TeamData>();
            foreach (var entity in entities)
            {
                var team = new TeamData
                {
                    Id = entity.TeamId,
                    Name = entity.Name,
                };
                result.Add(team);
            }

            return result;
        }
    }
}
