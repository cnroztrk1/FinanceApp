namespace FinanceApp.Common
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchFunction, TimeSpan expiration);
        void Remove(string key);
    }
}
