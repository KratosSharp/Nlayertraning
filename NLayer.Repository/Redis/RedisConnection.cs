using StackExchange.Redis;

namespace NLayer.Repository.Redis;

public class RedisConnection
{
    private readonly ConnectionMultiplexer _connection;

    public RedisConnection(string connectionString)
    {
        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase GetDatabase(int id)
    {
        return _connection.GetDatabase(id);
    }
    
}