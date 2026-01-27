using Microsoft.AspNetCore.Mvc;
using mh4webedit.Models;

namespace mh4webedit.Controllers;

public class CharacterController : Controller
{
    private readonly ICacheService _cacheService;

    public CharacterController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var save = _cacheService.GetSavegame();
        if (save == null)
        {
            return RedirectToAction("Index", "Upload");
        }

        var c = save.slot;
        var vm = new CharacterViewModel
        {
            Name = c.Name ?? string.Empty,
            HunterRank = c.HunterRank,
            HrPoints = c.HunterRankPoints,
            Funds = c.Funds
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(CharacterViewModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null)
        {
            return RedirectToAction("Index", "Upload");
        }

        var c = save.slot;
        c.Name = model.Name ?? string.Empty;
        c.HunterRank = model.HunterRank;
        c.HunterRankPoints = model.HrPoints;
        c.Funds = model.Funds;

        _cacheService.SetSavegame(save);

        TempData["Success"] = "Character updated.";
        return RedirectToAction("Index");
    }
}
