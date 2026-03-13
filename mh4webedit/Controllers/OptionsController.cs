using Microsoft.AspNetCore.Mvc;
using mh4edit;
using mh4webedit.Models;
using Microsoft.AspNetCore.Mvc.Routing;

namespace mh4webedit.Controllers;

[Route("api/[controller]")]
public class OptionsController : Controller
{
    private static readonly string s_armorNamesJson;
    private static readonly string s_armorEtag;
    private static readonly string s_jewelJson;
    private static readonly string s_jewelEtag;

    static OptionsController()
    {
        var names = new
        {
            chest = MonHunEquipStatic.allEqpChest,
            arms = MonHunEquipStatic.allEqpArms,
            waist = MonHunEquipStatic.allEqpWaist,
            legs = MonHunEquipStatic.allEqpLegs,
            heads = MonHunEquipStatic.allEqpHeads,
        };
        s_armorNamesJson = System.Text.Json.JsonSerializer.Serialize(names);
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(s_armorNamesJson));
        s_armorEtag = '"' + Convert.ToBase64String(hash) + '"';

        // prepare jewel options (value/display pairs)
        var jewelPairs = MonHunEquipStatic.Slot1Values.Select(s => new { value = s.Slot1, display = s.Display }).ToArray();
        s_jewelJson = System.Text.Json.JsonSerializer.Serialize(jewelPairs);
        var jhash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(s_jewelJson));
        s_jewelEtag = '"' + Convert.ToBase64String(jhash) + '"';
    }

    [HttpGet("ArmorNames")]
    public IActionResult ArmorNames()
    {
        var requestEtags = Request.Headers["If-None-Match"].ToString();
        if (!string.IsNullOrEmpty(requestEtags))
        {
            if (requestEtags.Split(',').Select(s => s.Trim()).Any(s => s == s_armorEtag))
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }
        }

        Response.Headers["ETag"] = s_armorEtag;
        return Content(s_armorNamesJson, "application/json");
    }

    [HttpGet("JewelOptions")]
    public IActionResult JewelOptions()
    {
        var requestEtags = Request.Headers["If-None-Match"].ToString();
        if (!string.IsNullOrEmpty(requestEtags))
        {
            if (requestEtags.Split(',').Select(s => s.Trim()).Any(s => s == s_jewelEtag))
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }
        }

        Response.Headers["ETag"] = s_jewelEtag;
        return Content(s_jewelJson, "application/json");
    }
}
