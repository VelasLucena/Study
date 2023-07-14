namespace SudyApi.Data.Interfaces
{
    public interface ICacheService
    {
        Task Set(string key, string valor);
        Task Remove(string key);
        Task<string> Get(string key);
    }
}
