using Microsoft.AspNetCore.Mvc;
using mh4edit;
using mh4webedit.Models;

namespace mh4webedit.Controllers;

public class ItemBoxController : Controller
{
    private readonly ICacheService _cacheService;
    public ItemBoxController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet]
    public IActionResult Index(int page = 1)
    {
        var save = _cacheService.GetSavegame();
        if (save == null)
        {
            return RedirectToAction("Index", "Upload");
        }

        var ch = save.slot;
        var vm = new ItemBoxViewModel { Page = page };
        int itemsPerPage = vm.ItemsPerPage;
        int start = (page - 1) * itemsPerPage;
        if (start < 0) start = 0;

        for (int i = start; i < System.Math.Min(start + itemsPerPage, ch.ItemBox.Length); i++)
        {
            var it = ch.ItemBox[i];
            vm.Items.Add(new ItemBoxItem { ItemName = it.Name, ItemId = it.ID, Quantity = it.Count });
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ItemBoxViewModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null)
        {
            return RedirectToAction("Index", "Upload");
        }

        var ch = save.slot;
        int start = (model.Page - 1) * model.ItemsPerPage;
        if (start < 0) start = 0;

        for (int i = 0; i < model.Items.Count; i++)
        {
            int idx = start + i;
            if (idx < 0 || idx >= ch.ItemBox.Length) continue;

            var vmItem = model.Items[i];
            ch.ItemBox[idx].Count = (ushort)(vmItem.Quantity > 99 ? 99 : vmItem.Quantity);

            // set ID from selected ItemId (index into names)
            if (vmItem.ItemId >= 0 && vmItem.ItemId < ch.ItemNames.Length)
            {
                ch.ItemBox[idx].ID = (ushort)vmItem.ItemId;
            }
        }

        _cacheService.SetSavegame(save);
        TempData["ItemBoxMessage"] = "Item box saved.";
        return RedirectToAction("Index", new { page = model.Page });
    }
}
