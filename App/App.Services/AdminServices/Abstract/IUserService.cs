﻿using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IUserService
{
    Task<Result<List<AllUsersDto>>> GetAllUsersAsync();
    Task<Result> ChangeActivenessOfUserAsync(int id);
}