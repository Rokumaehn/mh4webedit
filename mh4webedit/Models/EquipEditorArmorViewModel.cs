using System.ComponentModel.DataAnnotations;
using mh4edit;

namespace mh4webedit.Models;

public class EquipEditorArmorViewModel : EquipmentModel
{
    [Required]
    [Display(Name = "Upgrade Level")]
    [Range(0, 12)]
    public byte Upgrade { get; set; }

    [Required]
    public byte Resistances { get; set; }

    [Required]
    public byte Defense { get; set; }

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
}