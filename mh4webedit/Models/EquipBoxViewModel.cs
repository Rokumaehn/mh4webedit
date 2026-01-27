namespace mh4webedit.Models;

public class EquipBoxItem
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class EquipBoxViewModel
{
    public int Page { get; set; } = 1;
    public readonly int ItemsPerRow = 10;
    public readonly int RowsPerPage = 10;
    public readonly int ItemsPerPage = 100;
    public readonly int TotalItems = 1400;
    public List<EquipBoxItem> Items { get; set; } = new List<EquipBoxItem>();
}