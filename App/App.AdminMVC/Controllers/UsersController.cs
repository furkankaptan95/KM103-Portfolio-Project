using Microsoft.AspNetCore.Mvc;

namespace App.AdminMVC.Controllers;
public class UsersController : Controller
{
    [HttpGet]
    public async Task<IActionResult> User()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AllUsers()
    {
        return View();
    }

    [HttpGet]
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
    public async Task<IActionResult> UpdateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromForm] object updateUserModel)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DeleteUser()
    {
        return View();
    }

}
