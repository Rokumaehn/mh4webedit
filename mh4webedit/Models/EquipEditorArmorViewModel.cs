using System.ComponentModel.DataAnnotations;

namespace mh4webedit.Models;

public class EquipEditorArmorViewModel : EquipmentModel
{
    public int Type { get; set; }
    [Range(0, 12)]
    public byte Upgrade { get; set; }
    public byte Resistances { get; set; }
    public byte Defense { get; set; }
    public bool Polished { get; set; }
    public bool Glow { get; set; }
    public int NumSlots { get; set; }
    public byte Rarity { get; set; }
    public byte PolishReq { get; set; }
}