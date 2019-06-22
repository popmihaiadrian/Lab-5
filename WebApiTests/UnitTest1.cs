using Lab_2_webapi.Models;
using Lab_2_webapi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class Tests
    {

        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });
        }

        [Test]
        public void CreateANewUserAndVerifyExstence()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(CreateANewUserAndVerifyExstence))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new Lab_2_webapi.ViewModels.RegisterPostModel
                {
                    Email = "popmi@g",
                    FirstName = "Test1",
                    LastName = "Test2",
                    Password = "1234567",
                    Username = "test_username"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);
            }
        }

        [Test]
        public void AuthenticateShouldLoginAUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
             .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLoginAUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new Lab_2_webapi.ViewModels.RegisterPostModel

                {
                    FirstName = "pomihai2",
                    LastName = "popmihai2",
                    Username = "popmi2",
                    Email = "x@x",
                    Password = "popmihai"
                };
                var result = usersService.Register(added);
                var authenticated = new Lab_2_webapi.ViewModels.LoginPostModel
                {
                    Username = "popmi2",
                    Password = "popmihai"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);
            }
        }
        [Test]
        public void GetAllShouldReturnAllUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added1 = new Lab_2_webapi.ViewModels.RegisterPostModel

                {
                    FirstName = "pomihai2",
                    LastName = "popmihai2",
                    Username = "popmi2",
                    Email = "x@x",
                    Password = "popmihai"
                };
                var added2 = new Lab_2_webapi.ViewModels.RegisterPostModel

                {
                    FirstName = "pomihai2",
                    LastName = "popmihai2",
                    Username = "popmi3",
                    Email = "x@x",
                    Password = "popmihai"
                };

                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);
                Assert.AreEqual(2, numberOfElements);

            }
        }
    }
}
