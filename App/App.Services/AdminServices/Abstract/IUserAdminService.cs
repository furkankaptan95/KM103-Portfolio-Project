﻿using App.DTOs.UserDtos;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IUserAdminService
{
    Task<Result<List<AllUsersDto>>> GetAllUsersAsync();
    Task<Result> ChangeActivenessOfUserAsync(int id);
    Task<Result<string>> GetCommentsUserName(int id);
    Task<Result<int>> GetUsersCount();
}