﻿using App.Data.DbContexts;
using App.DTOs.UserDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AuthAPI.Services;
public class AdminUserService(AuthApiDbContext authApiDb) : IUserService
{
    public Task<Result> ChangeActivenessOfUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<AllUsersDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> GetCommentsUserName(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> GetUsersCount(int id)
    {
        throw new NotImplementedException();
    }
}