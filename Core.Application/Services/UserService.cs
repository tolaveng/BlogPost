﻿using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Core.Application.Utils;
using Core.Domain.Documents;
using Core.Domain.IRepositories;

namespace Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<User> mongoRespository;
        private readonly IMapper mapper;

        public UserService(IMongoRepository<User> mongoRespository, IMapper mapper)
        {
            this.mongoRespository = mongoRespository;
            this.mapper = mapper;
        }

        public async Task<int> CountUsersAsync()
        {
            var count = mongoRespository.AsQueryable().Count();
            return await Task.FromResult(count);
        }

        public async Task<bool> CreateUserAsyc(UserDto userDto)
        {
            try
            {
                var user = mapper.Map<User>(userDto);
                user.Id = MongoDB.Bson.ObjectId.GenerateNewId();
                var passwordHash = PasswordUtil.GenerateHashPassword(userDto.Password, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalf = passwordSalt;
                await mongoRespository.InsertOneAsync(user);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var user = mongoRespository.AsQueryable().ToList();
            return await Task.FromResult(mapper.Map<List<UserDto>>(user));
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await mongoRespository.FindOneAsync(x => x.Email.Equals(email));
            if (user == null) return null;
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await mongoRespository.FindOneAsync(x => x.Email.Equals(username));
            if (user == null) return null;
            return mapper.Map<UserDto>(user);
        }
    }
}
