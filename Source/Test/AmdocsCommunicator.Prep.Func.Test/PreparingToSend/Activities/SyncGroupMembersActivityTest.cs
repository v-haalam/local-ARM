﻿// <copyright file="SyncGroupMembersActivityTest.cs" company="Microsoft">
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
    using Amdocs.Teams.App.Communicator.Common.Repositories.UserData;
    using Amdocs.Teams.App.Communicator.Common.Resources;
    using Amdocs.Teams.App.Communicator.Common.Services.MicrosoftGraph;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using FluentAssertions;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using Moq;
    using Xunit;

    /// <summary>
    /// SyncGroupMembersActivity test class.
    /// </summary>
    public class SyncGroupMembersActivityTest
    {
        private readonly Mock<IGroupMembersService> groupMembersService = new Mock<IGroupMembersService>();
        private readonly Mock<IStringLocalizer<Strings>> localier = new Mock<IStringLocalizer<Strings>>();
        private readonly Mock<ILogger> logger = new Mock<ILogger>();
        private readonly Mock<IUserDataRepository> userDataRepository = new Mock<IUserDataRepository>();
        private readonly Mock<ISentNotificationDataRepository> sentNotificationDataRepository = new Mock<ISentNotificationDataRepository>();
        private readonly Mock<INotificationDataRepository> notificationDataRepository = new Mock<INotificationDataRepository>();

        /// <summary>
        /// Constructor tests.
        /// </summary>
        [Fact]
        public void ConstructorArgumentNullException_Test()
        {
            // Arrange
            Action action1 = () => new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, this.notificationDataRepository.Object, this.groupMembersService.Object, null /*userDataRepository*/, this.localier.Object);
            Action action2 = () => new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, this.notificationDataRepository.Object, this.groupMembersService.Object, this.userDataRepository.Object, null /*localier*/);
            Action action3 = () => new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, this.notificationDataRepository.Object, null /*groupMembersService*/, this.userDataRepository.Object, this.localier.Object);
            Action action4 = () => new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, null /*notificationDataRepository*/, this.groupMembersService.Object, this.userDataRepository.Object, this.localier.Object);
            Action action5 = () => new SyncGroupMembersActivity(null /*sentNotificationDataRepository*/, this.notificationDataRepository.Object, this.groupMembersService.Object, this.userDataRepository.Object, this.localier.Object);
            Action action6 = () => new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, this.notificationDataRepository.Object, this.groupMembersService.Object, this.userDataRepository.Object, this.localier.Object);

            // Act and Assert.
            action1.Should().Throw<ArgumentNullException>("userDataRepository is null.");
            action2.Should().Throw<ArgumentNullException>("localier is null.");
            action3.Should().Throw<ArgumentNullException>("groupMembersService is null.");
            action4.Should().Throw<ArgumentNullException>("notificationDataRepository is null.");
            action5.Should().Throw<ArgumentNullException>("sentNotificationDataRepository is null.");
            action6.Should().NotThrow();
        }

        /// <summary>
        /// Success Test to Syncs group members to repository.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task SyncGroupMembersActivitySuccessTest()
        {
            // Arrange
            var groupId = "Group1";
            var notificationId = "notificaionId";
            var activityContext = this.GetSyncGroupMembersActivity();
            var users = new List<User>()
            {
                new User() { Id = "userId" },
            };
            this.groupMembersService
                .Setup(x => x.GetGroupMembersAsync(It.IsAny<string>()))
                .ReturnsAsync(users);
            this.userDataRepository
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(default(UserDataEntity)));
            this.sentNotificationDataRepository
                .Setup(x => x.BatchInsertOrMergeAsync(It.IsAny<IEnumerable<SentNotificationDataEntity>>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await activityContext.RunAsync((notificationId, groupId), this.logger.Object);

            // Assert
            await task.Should().NotThrowAsync();
            this.sentNotificationDataRepository.Verify(x => x.BatchInsertOrMergeAsync(It.Is<IEnumerable<SentNotificationDataEntity>>(x => x.FirstOrDefault().PartitionKey == notificationId)));
        }

        /// <summary>
        /// ArgumentNullException Test.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [Fact]
        public async Task ArgumentNullExceptionTest()
        {
            // Arrange
            var groupId = "GroupId";
            var notificationId = "noticationId";
            var activityContext = this.GetSyncGroupMembersActivity();

            // Act
            Func<Task> task = async () => await activityContext.RunAsync((null /*notificationId*/, groupId), this.logger.Object);
            Func<Task> task1 = async () => await activityContext.RunAsync((notificationId, null /*groupId*/), this.logger.Object);
            Func<Task> task2 = async () => await activityContext.RunAsync((notificationId, groupId), null /*logger*/);

            // Assert
            await task.Should().ThrowAsync<ArgumentNullException>("notificationId is null");
            await task1.Should().ThrowAsync<ArgumentNullException>("groupId is null");
            await task2.Should().ThrowAsync<ArgumentNullException>("logger is null");
        }

        /// <summary>
        /// Initializes a new mock instance of the <see cref="SyncGroupMembersActivity"/> class.
        /// </summary>
        private SyncGroupMembersActivity GetSyncGroupMembersActivity()
        {
            return new SyncGroupMembersActivity(this.sentNotificationDataRepository.Object, this.notificationDataRepository.Object, this.groupMembersService.Object, this.userDataRepository.Object, this.localier.Object);
        }
    }
}
