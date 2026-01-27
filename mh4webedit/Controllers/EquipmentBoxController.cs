using Microsoft.AspNetCore.Mvc;
using mh4edit;
using mh4webedit.Models;

namespace mh4webedit.Controllers;

public class EquipmentBoxController : Controller
{
    private readonly ICacheService _cacheService;
    public EquipmentBoxController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpGet]
    public IActionResult Index(int page = 1, int? index = null)
    {
        var save = _cacheService.GetSavegame();
        if (save == null)
        {
            return RedirectToAction("Index", "Upload");
        }

        var ch = save.slot;

        if (index.HasValue)
        {
            int idx = index.Value;
            if (idx < 0 || idx >= ch.EquipBox.Length) return RedirectToAction("Index");

            var eq = ch.EquipBox[idx];
            if (eq == null) return RedirectToAction("Index");

            // types 1..5 are armor
            if (eq.Type >= 1 && eq.Type <= 5 && eq is MonHunArmor armor)
            {
                var vm = new EquipEditorArmorViewModel
                {
                    Type = eq.Type,
                    Index = idx,
                    ID = armor.ID,
                    Slot1 = armor.Slot1,
                    Slot1Fixed = armor.Slot1Fixed,
                    Slot2 = armor.Slot2,
                    Slot2Fixed = armor.Slot2Fixed,
                    Slot3 = armor.Slot3,
                    Slot3Fixed = armor.Slot3Fixed,
                    Upgrade = armor.Upgrade,
                    Resistances = armor.Resistances,
                    Defense = armor.Defense,
                    Polished = armor.Polished,
                    Glow = armor.Glow,
                    NumSlots = armor.NumSlots,
                    Rarity = armor.Rarity,
                    PolishReq = armor.PolishReq
                };

                return View("EditArmor", vm);
            }

            // not implemented: other editors
            return RedirectToAction("Index", new { page = page });
        }

        var vmList = new EquipBoxViewModel { Page = page };
        int itemsPerPage = vmList.ItemsPerPage;
        int start = (page - 1) * itemsPerPage;
        if (start < 0) start = 0;

        for (int i = start; i < System.Math.Min(start + itemsPerPage, ch.EquipBox.Length); i++)
        {
            var eq = ch.EquipBox[i];
            string name = eq?.IDString ?? "(None)";
            string img = eq?.GetImage()?.ToString() ?? "";
            if (!string.IsNullOrEmpty(img) && !img.StartsWith("/")) img = "/" + img.TrimStart('/');
            vmList.Items.Add(new EquipBoxItem { Index = i, Name = name, ImageUrl = img });
        }

        return View(vmList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditArmor(EquipEditorArmorViewModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ch = save.slot;
        if (model.Index < 0 || model.Index >= ch.EquipBox.Length) return RedirectToAction("Index");

        if (ch.EquipBox[model.Index] is MonHunArmor armor)
        {
            armor.ID = (ushort)model.ID;
            armor.Slot1 = model.Slot1;
            armor.Slot1Fixed = model.Slot1Fixed;
            armor.Slot2 = model.Slot2;
            armor.Slot2Fixed = model.Slot2Fixed;
            armor.Slot3 = model.Slot3;
            armor.Slot3Fixed = model.Slot3Fixed;
            armor.Upgrade = model.Upgrade;
            armor.Resistances = model.Resistances;
            armor.Defense = model.Defense;
            armor.Polished = model.Polished;
            armor.Glow = model.Glow;
            armor.NumSlots = model.NumSlots;
            armor.Rarity = model.Rarity;
            armor.PolishReq = model.PolishReq;

            _cacheService.SetSavegame(save);
            TempData["EquipBoxMessage"] = "Armor saved.";
        }

        return RedirectToAction("Index", new { page = 1 });
    }
}
