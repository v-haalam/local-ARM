// <copyright file="HealthControllerTest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace Amdocs.Teams.App.Communicator.Test.Controllers
{
    using Amdocs.Teams.App.Communicator.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    /// <summary>
    /// HealthController test class.
    /// </summary>
    public class HealthControllerTest
    {
        /// <summary>
        /// Test method to verify status code 200 for IndexAction.
        /// </summary>
        [Fact]
        public void Call_IndexAction_ReturnsStausCodeOk()
        {
            // Arrage
            var controller = this.GetHealthControllerInstance();
            var statusCodeOk = 200;

            // Act
            var result = controller.Index();
            var statusCode = ((StatusCodeResult)result).StatusCode;

            // Assert
            Assert.Equal(statusCode, statusCodeOk);
        }

        private HealthController GetHealthControllerInstance()
        {
            return new HealthController();
        }
    }
}
