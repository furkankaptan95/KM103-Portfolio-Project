using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class UsersController : Controller
{
    [HttpGet]
    [Route("user-{id:int}")]
    public async Task<IActionResult> User([FromRoute] int id)
    {
        return View();
    }

    [HttpGet]
    [Route("all-users")]
    public async Task<IActionResult> AllUsers()
    {
        return View();
    }

    [HttpGet]
    [Route("add-user")]
    public async Task<IActionResult> AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromForm] object addUserModel)
    {
        return View();
    }

    [HttpGet]
    [Route("update-user-{id:int}")]
    public async Task<IActionResult> UpdateUser([FromRoute] int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromForm] object updateUserModel)
    {
        return View();
    }

    [HttpGet]
    [Route("delete-user-{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        return View();
    }

}
