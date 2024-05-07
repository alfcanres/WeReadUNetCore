using AutoMapper;
using BusinessLogicLayer;
using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayerTest
{
    public class MoodTypeBLLTest
    {


        [Fact]
        public async Task CountAll_Should_Be_Valid()
        {
            //Arrange

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            var dbContext = new AppDbContext(dbOptions);

            await CreateTestDataAsync(dbContext);


            string expectedFirstMoodType = "Happy";
            string expectedSecondMoodType = "Sad";

            int expectedFirstMoodTypePostCount = 4;
            int expectedSecondMoodTypePostCount = 2;


            var unitOfWork = new UnitOfWork(dbContext);

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = mapperConfiguration.CreateMapper();
            IDataAnnotationsValidator validator = new DataAnnotationsValidatorHelper();
            var logger = new Mock<ILogger<MoodTypeBLL>>();

            MoodTypeBLL moodTypeBLL = new MoodTypeBLL(
                unitOfWork,
                mapper,
                logger.Object,
                validator
                );

            //Act
            var response = await moodTypeBLL.GetTopWithPostsAsync(5);


            //Assert
            string actualFirstMoodType = response.List.ToList()[0].Mood;
            string actualSecondMoodType = response.List.ToList()[1].Mood;

            int actualFirstMoodTypePostCount = response.List.ToList()[0].PostCount;
            int actualSecondMoodTypePostCount = response.List.ToList()[1].PostCount;

            Assert.Equal(actualFirstMoodType, expectedFirstMoodType);
            Assert.Equal(actualSecondMoodType, expectedSecondMoodType);


            Assert.Equal(actualFirstMoodTypePostCount, expectedFirstMoodTypePostCount);
            Assert.Equal(actualSecondMoodTypePostCount, expectedSecondMoodTypePostCount);
        }

        private async Task CreateTestDataAsync(AppDbContext dbContext)
        {
            ApplicationUserInfo userInfo = new ApplicationUserInfo();
            userInfo.UserID = Guid.NewGuid().ToString();
            userInfo.DateOfBirth = DateTime.Now;
            userInfo.FirstName = "TEST USER";
            userInfo.LastName = "TEST USER";
            userInfo.UserName = "test@test.com";

            dbContext.ApplicationUsers.Add(userInfo);


            var moodBrave = new MoodType() { IsAvailable = true, Mood = "Brave", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodBrave);
            
            var moodBlue = new MoodType() { IsAvailable = false, Mood = "Blue", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodBlue);

            var moodScared = new MoodType() { IsAvailable = false, Mood = "Scared", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodScared);

            var moodHappy = new MoodType() { IsAvailable = true, Mood = "Happy", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodHappy);

            var moodSad = new MoodType() { IsAvailable = true, Mood = "Sad", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodSad);

            var moodlove = new MoodType() { IsAvailable = false, Mood = "Love", Posts = new List<Post>() };
            dbContext.MoodTypes.Add(moodlove);


            var testPost1 = new Post { MoodType = moodHappy, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost1);
            
            var testPost2 = new Post { MoodType = moodHappy, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost2);

            var testPost3 = new Post { MoodType = moodHappy, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost3);

            var testPost4 = new Post { MoodType = moodHappy, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost4);

            var testPost5 = new Post { MoodType = moodSad, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost5);

            var testPost6 = new Post { MoodType = moodSad, ApplicationUserInfo = userInfo, Title = "Test", Text = "Test", CreationDate = DateTime.Now };
            dbContext.Posts.Add(testPost6);

            await dbContext.SaveChangesAsync();

        }



    }
}
