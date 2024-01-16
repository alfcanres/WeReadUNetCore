using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessLogicLayerTest
{
    public class AccountBLLTest
    {
        [Fact]
        public async Task CreateAsyncWithEmptyDataShouldNotBeValid()
        {

            //Arrange
            int expectedErrorsCount = 5;

            IdentityResult identityResult = IdentityResult.Failed(new[] { new IdentityError() { Description = "Just a test" } });

            var userManager = new Mock<IUserManagerWrapper>();
            userManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<String>())).Returns(Task.FromResult(identityResult));
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger>();

            AccountBLL accountBLL = new AccountBLL(userManager.Object, logger.Object, validator);

            //Act

            var result = await accountBLL.InsertAsync(new UserCreateDTO()
            {
                UserName = "",
                ComfirmPassword = "",
                Password = "",
                Email = "",
                FirstName = "",
                LastName = ""
            });

            string errorMessage = "Type a valid";

            //Assert
            int actualErrorsCount = result.ValidateResponse.MessageList.Where(t => t.Contains(errorMessage)).Count();

            Assert.Equal(expectedErrorsCount, actualErrorsCount);

        }
    }
}