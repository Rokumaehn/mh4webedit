using Microsoft.AspNetCore.Mvc;
using mh4edit;
using mh4webedit.Models;
using Microsoft.AspNetCore.Mvc.Routing;

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

            // types 0 is empty
            if (eq.Type == 0)
            {
                var vm = new EquipmentModel
                {
                    Type = eq.Type,
                    Index = idx,
                    ID = eq.ID,
                    ReturnPage = page.ToString(),
                };

                return View("EditType", vm);
            }
            // types 1..5 are armor
            else if (eq.Type >= 1 && eq.Type <= 5 && eq is MonHunArmor armor)
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
                    PolishReq = armor.PolishReq,
                    ReturnPage = page.ToString(),
                };

                return View("EditArmor", vm);
            }
            // type 6 is talisman
            else if (eq.Type == 6 && eq is MonHunTalisman talisman)
            {
                var vm = new EquipEditorTalismanViewModel
                {
                    Type = eq.Type,
                    Index = idx,
                    ID = talisman.ID,
                    Slot1 = talisman.Slot1,
                    Slot1Fixed = talisman.Slot1Fixed,
                    Slot2 = talisman.Slot2,
                    Slot2Fixed = talisman.Slot2Fixed,
                    Slot3 = talisman.Slot3,
                    Slot3Fixed = talisman.Slot3Fixed,
                    NumSlots = talisman.NumSlots,
                    Skill1ID = talisman.Skill1ID,
                    Skill1Amount = talisman.Skill1Amount,
                    Skill2ID = talisman.Skill2ID,
                    Skill2Amount = talisman.Skill2Amount,
                    
                    ReturnPage = page.ToString(),
                };

                return View("EditTalisman", vm);
            }
            // types 7..20 are weapons
            else if (eq.Type >= 7 && eq.Type <= 20 && eq is MonHunWeapon weapon)
            {
                var vm = new EquipEditorWeaponViewModel
                {
                    Index = idx,
                    Type = eq.Type,
                    ID = weapon.ID,
                    Slot1 = weapon.Slot1,
                    Slot1Fixed = weapon.Slot1Fixed,
                    Slot2 = weapon.Slot2,
                    Slot2Fixed = weapon.Slot2Fixed,
                    Slot3 = weapon.Slot3,
                    Slot3Fixed = weapon.Slot3Fixed,
                    ElementType = weapon.ElementType,
                    ElementValue = weapon.ElementValue,
                    Upgrade = weapon.Upgrade,
                    UpgradeValueName = "Upgrade Level",
                    UpgradeValues = weapon.UpgradeValueList,
                    Sharpness = weapon.Sharpness,
                    SharpnessValueName = weapon.SharpnessValueName,
                    SharpnessValues = weapon.SharpnessValueList,
                    Modifier = weapon.Modifier,
                    ModifierValueName = "Modifier",
                    ModifierValues = weapon.ModifierValueList,
                    Special = weapon.Special,
                    SpecialValueName = weapon.SpecialValueName,
                    SpecialValues = weapon.SpecialValueList,
                    Honing = weapon.Honing,
                    Polished = weapon.Polished,
                    Glow = weapon.Glow,
                    NumSlots = weapon.NumSlots,
                    Rarity = weapon.Rarity,
                    PolishReq = weapon.PolishReq,
                    ReturnPage = page.ToString(),
                };

                return View("EditWeapon", vm);
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
    public IActionResult EditType(EquipmentModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ch = save.slot;
        if (model.Index < 0 || model.Index >= ch.EquipBox.Length) return RedirectToAction("Index");

        var eqp = ch.EquipBox[model.Index];
        eqp.ID = (ushort)model.ID;
        eqp.Type = (byte)model.Type;
        var newEqp = MonHunEquip.Create(eqp.SerializeOnlyTypeAndId());
        ch.EquipBox[model.Index] = newEqp;

        _cacheService.SetSavegame(save);
        TempData["EquipBoxMessage"] = "Type changed.";

        return string.IsNullOrEmpty(model.ReturnPage) ? RedirectToAction("Index", new { page = 1, index = model.Index }) : RedirectToAction("Index", new { page = model.ReturnPage, index = model.Index });
    }

    [HttpGet]
    public IActionResult EditType(int page = 1, int index = -1)
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ch = save.slot;
        if (index < 0 || index >= ch.EquipBox.Length) return RedirectToAction("Index");

        var eq = ch.EquipBox[index];
        if (eq == null) return RedirectToAction("Index");

        var vm = new EquipmentModel
        {
            Type = eq.Type,
            Index = index,
            ID = eq.ID,
            ReturnPage = page.ToString(),
        };

        return View("EditType", vm);
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

        return string.IsNullOrEmpty(model.ReturnPage) ? RedirectToAction("Index", new { page = 1 }) : RedirectToAction("Index", new { page = model.ReturnPage });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditTalisman(EquipEditorTalismanViewModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ch = save.slot;
        if (model.Index < 0 || model.Index >= ch.EquipBox.Length) return RedirectToAction("Index");

        if (ch.EquipBox[model.Index] is MonHunTalisman talisman)
        {
            talisman.ID = (ushort)model.ID;
            talisman.Slot1 = model.Slot1;
            talisman.Slot1Fixed = model.Slot1Fixed;
            talisman.Slot2 = model.Slot2;
            talisman.Slot2Fixed = model.Slot2Fixed;
            talisman.Slot3 = model.Slot3;
            talisman.Slot3Fixed = model.Slot3Fixed;
            talisman.NumSlots = model.NumSlots;
            talisman.Skill1ID = model.Skill1ID;
            talisman.Skill1Amount = model.Skill1Amount;
            talisman.Skill2ID = model.Skill2ID;
            talisman.Skill2Amount = model.Skill2Amount;
            _cacheService.SetSavegame(save);
            TempData["EquipBoxMessage"] = "Talisman saved.";
        }

        return string.IsNullOrEmpty(model.ReturnPage) ? RedirectToAction("Index", new { page = 1 }) : RedirectToAction("Index", new { page = model.ReturnPage });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditWeapon(EquipEditorWeaponViewModel model)
    {
        var save = _cacheService.GetSavegame();
        if (save == null) return RedirectToAction("Index", "Upload");

        var ch = save.slot;
        if (model.Index < 0 || model.Index >= ch.EquipBox.Length) return RedirectToAction("Index");

        if (ch.EquipBox[model.Index] is MonHunWeapon weapon)
        {
            weapon.ID = (ushort)model.ID;
            weapon.Slot1 = model.Slot1;
            weapon.Slot1Fixed = model.Slot1Fixed;
            weapon.Slot2 = model.Slot2;
            weapon.Slot2Fixed = model.Slot2Fixed;
            weapon.Slot3 = model.Slot3;
            weapon.Slot3Fixed = model.Slot3Fixed;

            weapon.Upgrade = model.Upgrade;
            weapon.Sharpness = model.Sharpness;
            weapon.Modifier = model.Modifier;
            weapon.Special = model.Special;
            weapon.Honing = model.Honing;
            weapon.Polished = model.Polished;
            weapon.Glow = model.Glow;
            weapon.NumSlots = model.NumSlots;
            weapon.Rarity = model.Rarity;
            weapon.PolishReq = model.PolishReq;

            weapon.ElementType = model.ElementType;
            weapon.ElementValue = model.ElementValue;
            weapon.KinsectLevel = model.KinsectLevel;

            _cacheService.SetSavegame(save);
            TempData["EquipBoxMessage"] = "Weapon saved.";
        }

        return string.IsNullOrEmpty(model.ReturnPage) ? RedirectToAction("Index", new { page = 1 }) : RedirectToAction("Index", new { page = model.ReturnPage });
    }
}
