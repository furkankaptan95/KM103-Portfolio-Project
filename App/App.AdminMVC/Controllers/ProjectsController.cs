using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ProjectsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Project()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllProjects()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddProject()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProject([FromForm] object addProjectModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProject()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProject([FromForm] object updateProjectModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteProject()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> MakeProjectVisible()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> MakeProjectInVisible()
    {
        return View();
    }
}
