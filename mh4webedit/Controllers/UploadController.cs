using System;
using System.IO;
using System.Threading.Tasks;
using mh4edit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mh4webedit.Controllers;

public class UploadController : Controller
{
    private readonly ICacheService _cacheService;
    public UploadController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.Message = "No file selected.";
            ViewBag.Success = false;
            return View();
        }

        // try
        // {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var save = new MonHunSave(ms);
                save.FileName = file.FileName ?? string.Empty;
                _cacheService.SetSavegame(save);
            }

            ViewBag.Message = $"Uploaded {file.FileName}";
            ViewBag.Success = true;
            ViewBag.SavedPath = "<memory>";

            // mark session so layout can show upload-specific nav items
            HttpContext.Session.SetString("HasUploadedFile", "true");
            HttpContext.Session.SetString("UploadedFileName", file.FileName ?? string.Empty);
            return View();
        // }
        // catch (Exception ex)
        // {
        //     ViewBag.Message = "Upload failed: " + ex.Message;
        //     ViewBag.Success = false;
        //     return View();
        // }
    }
}
