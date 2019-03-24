﻿using System;
using System.Threading;
using UserGoldService.Adapters;
using UserGoldService.Entities;
namespace UserGoldService.Domain
{
    public interface IUserService
    {
        Result<decimal> GetMyGold(string token);
        Result<bool> AddGold(decimal count, string token);
        Result<string> Register(string userName);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Result<bool> AddGold(decimal count, string token)
        {
            if (count < 0)
            {
                return ErroWithMessage<bool>("count < 0");
            }

            if (!Guid.TryParse(token, out Guid id))
            {
                return NotValidToken<bool>();
            }
            var mutex = new Mutex(false, token);
            mutex.WaitOne();
            try
            {
                User user = _repository.Get(id);
            if (user == null)
            {
                return NotValidToken<bool>();
            }

            if (decimal.MaxValue - count <= user.Gold)
            {
                return ErroWithMessage<bool>($"count to big. count:{count} + user gold:{user.Gold} must by < {decimal.MaxValue}");
            }

            user.Gold += count;
            _repository.Update(user);
            return Valid<bool>(true);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public Result<decimal> GetMyGold(string token)
        {
            if (!Guid.TryParse(token, out Guid id))
            {
                return NotValidToken<decimal>();
            }

            User user = _repository.Get(id);
            if (user == null)
            {
                return NotValidToken<decimal>();
            }
            return Valid<decimal>(user.Gold);
        }

        public Result<string> Register(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return ErroWithMessage<string>("user name is empty!");
            var mutex = new Mutex(false, userName);
            mutex.WaitOne();
            try
            {
                var user = _repository.GetByName(userName);
                if (user == null)
                {
                    user = new User
                    {
                        id = Guid.NewGuid(),
                        Name = userName,
                        Gold = 0
                    };
                    _repository.Create(user);
                }
                return Valid<string>(user.id.ToString());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private Result<T> NotValidToken<T>()
        {
            return new Result<T>(default(T), ErrorKind.NotValidToken, null);
        }

        private Result<T> ErroWithMessage<T>(string message)
        {
            return new Result<T>(default(T), ErrorKind.NotValidToken, message);
        }

        private Result<T> Valid<T>(T value)
        {
            return new Result<T>(value, ErrorKind.None, null);
        }
    }
}
