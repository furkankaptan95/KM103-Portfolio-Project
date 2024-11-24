﻿using App.DTOs.ProjectDtos;
using App.DTOs.ProjectDtos.Admin;
using Ardalis.Result;

namespace App.Services.AdminServices.Abstract;
public interface IProjectAdminService
{
    Task<Result<List<AllProjectsAdminDto>>> GetAllProjectsAsync();
    Task<Result> AddProjectAsync(AddProjectApiDto dto);
    Task<Result> AddProjectAsync(AddProjectMVCDto dto);
    Task<Result> UpdateProjectAsync(UpdateProjectMVCDto dto);
    Task<Result> UpdateProjectAsync(UpdateProjectApiDto dto);
    Task<Result<ProjectToUpdateDto>> GetByIdAsync(int id);
    Task<Result> DeleteProjectAsync(int id);
    Task<Result> ChangeProjectVisibilityAsync(int id);
}
