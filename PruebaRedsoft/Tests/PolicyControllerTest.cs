using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PruebaRedsoft.Controllers;
using PruebaRedsoft.Models;
using PruebaRedsoft.Services;
using System.Security.Claims;

namespace PruebaRedsoft.Tests
{
    [TestClass]
    public class PolicyControllerTests
    {
        private Mock<IPolicyService> mockPolicyService;
        private Mock<IJwtService> mockJwtService;
        private PolicyController policyController;

        [TestInitialize]
        public void TestInitialize()
        {
            mockPolicyService = new Mock<IPolicyService>();
            mockJwtService = new Mock<IJwtService>();

            // Mock ClaimsIdentity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test user"),
                // Add other claims as needed
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            policyController = new PolicyController(mockPolicyService.Object, mockJwtService.Object);

            // Setup the controller context
            policyController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Set up mockJwtService to return success
            mockJwtService.Setup(service => service.ValidateToken(It.IsAny<ClaimsIdentity>()))
                .Returns(new { success = true });
        }

        [TestMethod]
        public void Get_ReturnsPolicyList_WhenPoliciesExist()
        {
            // Arrange
            var expectedPolicies = new List<Policy> { new Policy { NumberPolicy = 123 } };
            mockPolicyService.Setup(service => service.Get()).Returns(expectedPolicies);

            // Act
            var result = policyController.Get();

            // Assert
            Assert.AreEqual(expectedPolicies, result.Value);
        }

        [TestMethod]
        public void Get_ReturnsPolicy_WhenPolicyExists()
        {
            // Arrange
            var id = "123";
            var expectedPolicy = new Policy { Id = id };
            mockPolicyService.Setup(service => service.Get(id)).Returns(expectedPolicy);

            // Act
            var result = policyController.Get(id);

            // Assert
            Assert.AreEqual(expectedPolicy, result.Value);
        }

        [TestMethod]
        public void Post_ReturnsCreatedAtAction_WhenPolicyIsCreated()
        {
            // Arrange
            var policy = new Policy();
            mockPolicyService.Setup(service => service.Create(policy));

            // Act
            var result = policyController.Post(policy);

            // Assert
            mockPolicyService.Verify(service => service.Create(policy), Times.Once);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("Get", createdAtActionResult.ActionName);
            Assert.AreEqual(policy, createdAtActionResult.Value);
        }
    } 
}

