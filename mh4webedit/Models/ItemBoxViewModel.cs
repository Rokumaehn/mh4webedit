namespace mh4webedit.Models;
public class ItemBoxItem
{
    public string ItemName { get; set; } = string.Empty;
    public int ItemId { get; set; }
    public uint Quantity { get; set; }
}

public class ItemBoxViewModel
{
    public int Page { get; set; } = 1;
    public readonly int ItemsPerPage = 10;
    public readonly int TotalItems = 1400;
    public List<ItemBoxItem> Items { get; set; } = new List<ItemBoxItem>();
}