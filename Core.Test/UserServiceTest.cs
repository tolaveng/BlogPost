using AutoMapper;
using Core.Application.Mapper;
using Core.Application.Services;
using Core.Application.Services.Interfaces;
using Core.Domain.Documents;
using Core.Domain.IRepositories;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test
{
    public class UserServiceTest
    {
        private readonly IUserService userService;
        private IMapper mapper;

        public UserServiceTest()
        {
            var userId = ObjectId.GenerateNewId();
            var users = new List<User>()
            {
                new User()
                {
                    Id = userId,
                    Username = "test",
                    Email = "test@test.com"
                }
            };

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            this.mapper = mappingConfig.CreateMapper();

            Mock<IMongoRepository<User>> mongoUserMock = new Mock<IMongoRepository<User>>();

            mongoUserMock.Setup(x => x.AsQueryable()).Returns(users.AsQueryable());

            mongoUserMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> f) => {
                    var user = users.AsQueryable().SingleOrDefault(f);
                    if (user == null) return Task.FromResult<User>(null);
                    return Task.FromResult(user);
                });

            this.userService = new UserService(mongoUserMock.Object, mapper);
        }

        [Fact]
        public async Task ShouldReturnAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            Assert.Single(users);
        }

        [Fact]
        public async Task ShouldReturnUserByEmail()
        {
            var user = await userService.GetUserByEmailAsync("test@test.com");
            Assert.True(user != null);
            Assert.Equal("test@test.com", user.Email);
        }

        [Fact]
        public async Task ShouldReturnNullUserEmailNotFound()
        {
            var user = await userService.GetUserByEmailAsync("no@test.com");
            Assert.True(user == null);
        }
    }
}
