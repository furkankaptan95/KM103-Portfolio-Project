using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class ProjectsController : Controller
{
    [HttpGet]
    [Route("project-{id:int}")]
    public async Task<IActionResult> Project([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("all-projects")]
    public async Task<IActionResult> AllProjects()
    {
        return View();
    }

    [HttpGet]
    [Route("add-project")]
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
    [Route("update-project-{id:int}")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProject([FromForm] object updateProjectModel)
    {
        return View();
    }

    [HttpGet]
    [Route("delete-project-{id:int}")]
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {
        return View();
    }


    [HttpGet]
    [Route("make-project-visible-{id:int}")]
    public async Task<IActionResult> MakeProjectVisible([FromRoute] int id)
    {
        return View();
    }


    [HttpGet]
    [Route("make-project-invisible-{id:int}")]
    public async Task<IActionResult> MakeProjectInVisible([FromRoute] int id)
    {
        return View();
    }
}
