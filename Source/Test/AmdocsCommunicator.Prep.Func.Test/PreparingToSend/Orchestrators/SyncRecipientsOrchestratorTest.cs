﻿// <copyright file="SyncRecipientsOrchestratorTest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Prep.Func.Test.PreparingToSend.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amdocs.Teams.App.Communicator.Common.Repositories.NotificationData;
    using Amdocs.Teams.App.Communicator.Prep.Func.PreparingToSend;
    using FluentAssertions;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    /// <summary>
    /// SyncRecipientsOrchestrator test class.
    /// </summary>
    public class SyncRecipientsOrchestratorTest
    {
        private readonly Mock<IDurableOrchestrationContext> mockContext = new Mock<IDurableOrchestrationContext>();
        private readonly Mock<ILogger> mockLogger = new Mock<ILogger>();

        /// <summary>
        /// Syncs all set of recipients to repository.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task SyncRecipientsOrchestratorGetAllUsersTest()
        {
            // Arrange
            NotificationDataEntity notificationDataEntity = new NotificationDataEntity()
            {
                Id = "notificationId",
                AllUsers = true,
            };

            this.mockContext
                .Setup(x => x.GetInput<NotificationDataEntity>())
                .Returns(notificationDataEntity);
            this.mockContext
                .Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);
            this.mockContext
                .Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<NotificationDataEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await SyncRecipientsOrchestrator.RunOrchestrator(this.mockContext.Object, this.mockLogger.Object);

            // Assert
            await task.Should().NotThrowAsync<ArgumentException>();
            this.mockContext.Verify(x => x.CallActivityWithRetryAsync(It.Is<string>(x => x.Equals(FunctionNames.SyncAllUsersActivity)), It.IsAny<RetryOptions>(), It.Is<NotificationDataEntity>(x => x.AllUsers))); // Allusers flag is true
            this.mockContext.Verify(x => x.CallActivityWithRetryAsync(It.Is<string>(x => x.Equals(FunctionNames.UpdateNotificationStatusActivity)), It.IsAny<RetryOptions>(), It.IsAny<object>()));
        }

        /// <summary>
        /// Syncs Members of specific teams to repository.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task SyncRecipientsOrchestratorGetMembersOfSpecifictTeamTest()
        {
            // Arrange
            NotificationDataEntity notificationDataEntity = new NotificationDataEntity()
            {
                Id = "notificationId",
                AllUsers = false,
                Rosters = new List<string>() { "roaster", "roaster1" },
            };

            this.mockContext
                .Setup(x => x.GetInput<NotificationDataEntity>())
                .Returns(notificationDataEntity);
            this.mockContext
                .Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await SyncRecipientsOrchestrator.RunOrchestrator(this.mockContext.Object, this.mockLogger.Object);

            // Assert
            await task.Should().NotThrowAsync<ArgumentException>();
            this.mockContext
                .Verify(x => x.CallActivityWithRetryAsync(It.Is<string>(x => x.Equals(FunctionNames.SyncTeamMembersActivity)), It.IsAny<RetryOptions>(), It.IsAny<object>()), Times.Exactly(notificationDataEntity.Rosters.Count()));
        }

        /// <summary>
        /// Syncs Members of M365 groups, DG or SG to repository.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task SyncRecipientsOrchestratorGetMembersOfGroupsTest()
        {
            // Arrange
            NotificationDataEntity notificationDataEntity = new NotificationDataEntity()
            {
                Id = "notificationId",
                AllUsers = false,
                Rosters = new List<string>(),
                Groups = new List<string>() { "Group1", "Group2" },
            };

            this.mockContext
                .Setup(x => x.GetInput<NotificationDataEntity>())
                .Returns(notificationDataEntity);
            this.mockContext
                .Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await SyncRecipientsOrchestrator.RunOrchestrator(this.mockContext.Object, this.mockLogger.Object);

            // Assert
            await task.Should().NotThrowAsync<ArgumentException>();
            this.mockContext
                .Verify(x => x.CallActivityWithRetryAsync(It.Is<string>(x => x.Equals(FunctionNames.SyncGroupMembersActivity)), It.IsAny<RetryOptions>(), It.IsAny<object>()), Times.Exactly(notificationDataEntity.Groups.Count()));
        }

        /// <summary>
        /// Syncs Members of general channel to repository.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchffronous operation.</returns>
        [Fact]
        public async Task SyncRecipientsOrchestratorGetMembersOfGeneralChannelTest()
        {
            // Arrange
            NotificationDataEntity notificationDataEntity = new NotificationDataEntity()
            {
                Id = "notificationId",
                AllUsers = false,
                Rosters = new List<string>(),
                Groups = new List<string>(),
                Teams = new List<string>() { "TestTeamChannel" },
            };

            this.mockContext
                .Setup(x => x.GetInput<NotificationDataEntity>())
                .Returns(notificationDataEntity);
            this.mockContext
                .Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await SyncRecipientsOrchestrator.RunOrchestrator(this.mockContext.Object, this.mockLogger.Object);

            // Assert
            await task.Should().NotThrowAsync<ArgumentException>();
            this.mockContext
                .Verify(x => x.CallActivityWithRetryAsync(It.Is<string>(x => x.Equals(FunctionNames.SyncTeamsActivity)), It.IsAny<RetryOptions>(), It.IsAny<object>()), Times.Exactly(1));
        }

        /// <summary>
        /// Sync recipients for invalid Audience.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task SyncRecipientsOrchestratorForInvalidAudienceSelectionTest()
        {
            // Arrange
            NotificationDataEntity notificationDataEntity = new NotificationDataEntity()
            {
                Id = "notificationId",
                AllUsers = false,
                Rosters = new List<string>(),
                Groups = new List<string>(),
                Teams = new List<string>(),
            };

            this.mockContext.Setup(x => x.GetInput<NotificationDataEntity>()).Returns(notificationDataEntity);
            this.mockContext.Setup(x => x.CallActivityWithRetryAsync(It.IsAny<string>(), It.IsAny<RetryOptions>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> task = async () => await SyncRecipientsOrchestrator.RunOrchestrator(this.mockContext.Object, this.mockLogger.Object);

            // Assert
            await task.Should().ThrowAsync<ArgumentException>($"Invalid audience select for notification id: {notificationDataEntity.Id}");
        }
    }
}