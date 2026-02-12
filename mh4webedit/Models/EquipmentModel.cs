namespace mh4webedit.Models;

public class EquipmentModel
{
    public int Index { get; set; }
    public int Type { get; set; }
    public int ID { get; set; }
    public ushort Slot1 { get; set; }
    public bool Slot1Fixed { get; set; }
    public ushort Slot2 { get; set; }
    public bool Slot2Fixed { get; set; }
    public ushort Slot3 { get; set; }
    public bool Slot3Fixed { get; set; }
    public string ReturnPage { get; set; } = string.Empty;
}