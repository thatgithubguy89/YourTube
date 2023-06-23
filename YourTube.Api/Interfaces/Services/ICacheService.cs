namespace YourTube.Api.Interfaces.Services
{
    public interface ICacheService<T> where T : class
    {
        void SetItem(string key, T value);
        void SetItems(string key, List<T> values);
        void DeleteItems(string key);
        T GetItem(string key);
        List<T> GetItems(string key);
    }
}
