﻿using App.DTOs.PersonalInfoDtos;
using App.Services.AdminServices.Abstract;
using Ardalis.Result;

namespace App.AdminMVC.Services;
public class PersonalInfoService(IHttpClientFactory factory) : IPersonalInfoService
{
    private HttpClient DataApiClient => factory.CreateClient("dataApi");
    public Task<Result> AddPersonalInfoAsync(AddPersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ShowPersonalInfoDto>> GetPersonalInfoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdatePersonalInfoAsync(UpdatePersonalInfoDto dto)
    {
        throw new NotImplementedException();
    }
}