
using Microsoft.Extensions.Caching.Memory;
using mh4edit;

public interface ICacheService
{
    public void SetItem(string key, object value);
    public T? GetItem<T>(string key) where T : class;

    public bool HasSavegame();
    public MonHunSave? GetSavegame();
    public void SetSavegame(MonHunSave savegame);
}
