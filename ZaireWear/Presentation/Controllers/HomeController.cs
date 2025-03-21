﻿using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

public class HomeController : Controller
{
    private readonly ISliderService _sliderService;

    public HomeController(ISliderService sliderService)
    {
        _sliderService = sliderService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _sliderService.GetAllAsync();
        model.Sliders = model.Sliders.Where(s => s.IsActive).ToList();
        return View(model);
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult ContactUs()
    {
        return View();
    }
}
