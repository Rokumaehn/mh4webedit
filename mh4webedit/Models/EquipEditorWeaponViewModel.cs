using System.ComponentModel.DataAnnotations;
using mh4edit;

namespace mh4webedit.Models;

public class EquipEditorWeaponViewModel : EquipmentModel
{
    // if a weapon is ranged, it has no element type and value.
    public bool IsRanged => Type is 11 or 12 or 16;
    public bool IsBow => Type == 16;
    public bool IsLbg => Type == 11;
    public bool IsHbg => Type == 12;
    // only the glaive uses kinsect level field.
    public bool IsGlaive => Type == 19;

    public byte ElementType { get; set; }
    public byte ElementValue { get; set; }

    [Required]
    public byte Sharpness {get; set; }
    public string SharpnessValueName {get; init; }
    public List<MonHunEquipStatic.SharpnessValueType> SharpnessValues { get; set; }

    [Required]
    public byte Modifier { get; set; }
    public string ModifierValueName { get; init; }
    public List<MonHunEquipStatic.ModifierValueType> ModifierValues { get; set; }

    [Required]
    public byte Special { get; set; }
    public string SpecialValueName { get; init; }
    public List<MonHunEquipStatic.SpecialValueType> SpecialValues { get; set; }

    [Required]
    public byte Upgrade { get; set; }
    public string UpgradeValueName { get; init; }
    public List<MonHunEquipStatic.UpgradeValueType> UpgradeValues { get; set; }

    [Required]
    [Range(0, 3)]
    public byte Honing { get; set; }
    [Required]
    public bool Polished { get; set; }
    [Required]
    public bool Glow { get; set; }
    [Range(0, 3)]
    public int NumSlots { get; set; }
    [Range(1, 10)]
    public byte Rarity { get; set; }

    [Display(Name = "Polish Req")]
    [Range(0, 4)]
    public byte PolishReq { get; set; }

    public byte KinsectLevel { get; set; }
}