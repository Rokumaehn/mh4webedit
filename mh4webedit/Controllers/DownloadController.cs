using Microsoft.AspNetCore.Mvc;
using mh4edit;

namespace mh4webedit.Controllers;

public class DownloadController : Controller
{
    private readonly ICacheService _cacheService;
    public DownloadController(ICacheService cacheService)
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

        return View(save);
    }

    [HttpGet]
    public IActionResult Download()
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ms = save.Save();
        ms.Seek(0, System.IO.SeekOrigin.Begin);
        var fileName = string.IsNullOrEmpty(save.FileName) ? "save.dat" : save.FileName;
        return File(ms, "application/octet-stream", fileName);
    }
}
