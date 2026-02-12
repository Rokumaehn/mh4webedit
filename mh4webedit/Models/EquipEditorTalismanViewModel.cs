using System.ComponentModel.DataAnnotations;

namespace mh4webedit.Models;

public class EquipEditorTalismanViewModel : EquipmentModel
{
    [Range(0, 3)]
    public int NumSlots { get; set; }
    [Required]
    public ushort Skill1ID { get; set; }
    [Required]
    [Range(0, 10)]
    public ushort Skill1Amount { get; set; }
    public ushort Skill2ID { get; set; }
    [Range(0, 10)]
    public ushort Skill2Amount { get; set; }
}