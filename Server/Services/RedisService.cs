using StackExchange.Redis;
using System.Text.Json;

namespace Server.Services;

public interface IRedisService
{
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null);

    Task<T?> GetAsync<T>(string key);

    Task DeleteAsync(string key);
}

public class RedisService : IRedisService
{
    private readonly IDatabase _redis;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);

        await _redis.StringSetAsync(key, json);

        if (expiry.HasValue)
        {
            await _redis.KeyExpireAsync(key, expiry.Value);
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _redis.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task DeleteAsync(string key)
    {
        await _redis.KeyDeleteAsync(key);
    }
}