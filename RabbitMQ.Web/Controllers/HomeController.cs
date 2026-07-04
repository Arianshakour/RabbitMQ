using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Application.Interface;
using RabbitMQ.Application.ViewModels;
using RabbitMQ.Web.Models;

namespace RabbitMQ.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger,IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _userService.RegisterAsync(model);

        ViewBag.Message = "ثبت نام انجام شد.";

        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
