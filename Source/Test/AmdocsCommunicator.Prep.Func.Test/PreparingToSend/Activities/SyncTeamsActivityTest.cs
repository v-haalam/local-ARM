﻿// <copyright file="SyncTeamsActivityTest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.Test.PreparingToSend.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Common.Repositories.SentNotificationData;
    using Amdocs.Teams.App.Communicator.Common.Repositories.TeamData;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using FluentAssertions;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// SyncTeamsActivity test class.
    /// </summary>
    public class SyncTeamsActivityTest
    {
        private readonly Mock<IStringLocalizer<Strings>> localier = new Mock<IStringLocalizer<Strings>>();
        private readonly Mock<ILogger> log = new Mock<ILogger>();
        private readonly Mock<ISentNotificationDataRepository> sentNotificationDataRepository = new Mock<ISentNotificationDataRepository>();
        private readonly Mock<INotificationDataRepository> notificationDataRepository = new Mock<INotificationDataRepository>();
        private readonly Mock<ITeamDataRepository> teamDataRepository = new Mock<ITeamDataRepository>();

        /// <summary>
        /// Constructor test.
        /// </summary>
        [Fact]
        public void SyncTeamsActivityConstructorTest()
        {
            // Arrange
            Action action1 = () => new SyncTeamsActivity(null /*teamDataRepository*/, this.sentNotificationDataRepository.Object, this.localier.Object, this.notificationDataRepository.Object);
            Action action2 = () => new SyncTeamsActivity(this.teamDataRepository.Object, null /*sentNotificationDataRepository*/, this.localier.Object, this.notificationDataRepository.Object);
            Action action3 = () => new SyncTeamsActivity(this.teamDataRepository.Object, this.sentNotificationDataRepository.Object, null /*localier*/, this.notificationDataRepository.Object);
            Action action4 = () => new SyncTeamsActivity(this.teamDataRepository.Object, this.sentNotificationDataRepository.Object, this.localier.Object, null /*notificationDataRepository*/);
            Action action5 = () => new SyncTeamsActivity(this.teamDataRepository.Object, this.sentNotificationDataRepository.Object, this.localier.Object, this.notificationDataRepository.Object);

            // Act and Assert.
            action1.Should().Throw<ArgumentNullException>("teamDataRepository is null.");
            action2.Should().Throw<ArgumentNullException>("sentNotificationDataRepository is null.");
            action3.Should().Throw<ArgumentNullException>("localier is null.");
            action4.Should().Throw<ArgumentNullException>("notificationDataRepository is null.");
            action5.Should().NotThrow();
        }

        /// <summary>
        /// Sync Teams activity success test.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task SyncTeamsActivitySuccessTest()
        {
            // Arrange
            var activityContext = this.GetSyncTamActivity();
            IEnumerable<string> roasters = new List<string>() { "teamId1", "teamId2" };
            NotificationDataEntity notification = new NotificationDataEntity()
            {
                Id = "notificationId",
                Rosters = roasters,
                TeamsInString = "['teamId1','teamId2']",
            };

            IEnumerable<TeamDataEntity> teamData = new List<TeamDataEntity>()
            {
                new TeamDataEntity() { TeamId = "teamId1" },
                new TeamDataEntity() { TeamId = "teamId2" },
            };

            this.teamDataRepository
                .Setup(x => x.GetTeamDataEntitiesByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(teamData);
            this.notificationDataRepository
                .Setup(x => x.SaveWarningInNotificationDataEntityAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            this.sentNotificationDataRepository
                .Setup(x => x.BatchInsertOrMergeAsync(It.IsAny<IEnumerable<SentNotificationDataEntity>>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await activityContext.RunAsync(notification, this.log.Object);

            // Assert
            await task.Should().NotThrowAsync();
            this.sentNotificationDataRepository.Verify(x => x.BatchInsertOrMergeAsync(It.Is<IEnumerable<SentNotificationDataEntity>>(
                x => x.Count() == 2)));
            this.notificationDataRepository.Verify(x => x.SaveWarningInNotificationDataEntityAsync(It.Is<string>(x => x.Equals(notification.Id)), It.IsAny<string>()), Times.Never());
        }

        /// <summary>
        /// Sync teams data to Sent notification repository. Save warning message logged for each team that is absent in repository.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task SyncTeamsActivitySuccessWithSaveWarningNotificationTest()
        {
            // Arrange
            var activityContext = this.GetSyncTamActivity();
            IEnumerable<string> roasters = new List<string>() { "teamId1", "teamId2" };
            NotificationDataEntity notification = new NotificationDataEntity()
            {
                Id = "123",
                Rosters = roasters,
                TeamsInString = "['teamId1','teamId2']",
            };
            IEnumerable<TeamDataEntity> teamData = new List<TeamDataEntity>()
            {
                new TeamDataEntity() { TeamId = "teamId1" },
            };

            this.teamDataRepository
                .Setup(x => x.GetTeamDataEntitiesByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(teamData);
            this.notificationDataRepository
                .Setup(x => x.SaveWarningInNotificationDataEntityAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            this.sentNotificationDataRepository
                .Setup(x => x.BatchInsertOrMergeAsync(It.IsAny<IEnumerable<SentNotificationDataEntity>>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await activityContext.RunAsync(notification, this.log.Object);

            // Assert
            await task.Should().NotThrowAsync();
            this.sentNotificationDataRepository.Verify(x => x.BatchInsertOrMergeAsync(It.Is<IEnumerable<SentNotificationDataEntity>>(x => x.Count() == 1)));

            // Warn message should be logged once for "teamId2".
            this.notificationDataRepository.Verify(x => x.SaveWarningInNotificationDataEntityAsync(It.Is<string>(x => x.Equals(notification.Id)), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// SyncTeamsActivity argumentNullException test.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task SyncTeamsActivityNullArgumentTest()
        {
            // Arrange
            var activityContext = this.GetSyncTamActivity();
            NotificationDataEntity notification = new NotificationDataEntity()
            {
                Id = "notificationId",
            };
            IEnumerable<TeamDataEntity> teamData = new List<TeamDataEntity>();
            this.teamDataRepository.Setup(x => x.GetTeamDataEntitiesByIdsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(teamData);

            // Act
            Func<Task> task1 = async () => await activityContext.RunAsync(null /*notification*/, null/*logger*/);
            Func<Task> task2 = async () => await activityContext.RunAsync(null /*notification*/, this.log.Object);
            Func<Task> task3 = async () => await activityContext.RunAsync(notification, null /*logger*/);

            // Assert
            await task1.Should().ThrowAsync<ArgumentNullException>();
            await task2.Should().ThrowAsync<ArgumentNullException>("notification is null");
            await task3.Should().ThrowAsync<ArgumentNullException>("logger is null");
        }

        /// <summary>
        /// Initializes a new mock instance of the <see cref="SyncTeamsActivity"/> class.
        /// </summary>
        private SyncTeamsActivity GetSyncTamActivity()
        {
            return new SyncTeamsActivity(this.teamDataRepository.Object, this.sentNotificationDataRepository.Object, this.localier.Object, this.notificationDataRepository.Object);
        }
    }
}
