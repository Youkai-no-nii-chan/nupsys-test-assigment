using MongoDB.Driver;

namespace Model;

public class DataRepository
{
    private readonly IMongoCollection<Data> _collection;

    public DataRepository()
    {
        var dbClient = new MongoClient("mongodb://localhost:27017");

        var database = dbClient.GetDatabase("nupsys-test");
        _collection = database.GetCollection<Data> ("data");
    }

    public async Task InsertAsync(Data data)
    {
        await _collection.InsertOneAsync(data);
    }

    public async Task<IAsyncCursor<Data>> GetFlatList(DateTime? periodStart, DateTime? periodEnd)
    {
        return await _collection.FindAsync(BuildFilter(periodStart, periodEnd));
    }

    public async Task<IAsyncCursor<ConsolidatedData>> ConsolidateHours(DateTime? periodStart, DateTime? periodEnd)
    {
        return await _collection
            .Aggregate()
            .Match(BuildFilter(periodStart, periodEnd))
            .Group(
                data => new DateTime(data.Timestamp.Year, data.Timestamp.Month, data.Timestamp.Day, data.Timestamp.Hour, 0, 0), 
                group => new ConsolidatedData
                {
                    Timestamp = group.Key,
                    Value = group.Average(data => data.Value)
                })
            .Sort(Builders<ConsolidatedData>.Sort.Ascending(data => data.Timestamp))
            .ToCursorAsync();
    }

    public async Task<IAsyncCursor<ConsolidatedData>> ConsolidateMinutes(DateTime? periodStart, DateTime? periodEnd)
    {
        return await _collection
            .Aggregate()
            .Match(BuildFilter(periodStart, periodEnd))
            .Group(
                data => new DateTime(data.Timestamp.Year, data.Timestamp.Month, data.Timestamp.Day, data.Timestamp.Hour, data.Timestamp.Minute, 0), 
                group => new ConsolidatedData
                {
                    Timestamp = group.Key,
                    Value = group.Average(data => data.Value)
                })
            .Sort(Builders<ConsolidatedData>.Sort.Ascending(data => data.Timestamp))
            .ToCursorAsync();
    }

    public async Task<IAsyncCursor<ConsolidatedData>> PeaksPerHour(DateTime? periodStart, DateTime? periodEnd)
    {
        return await _collection
            .Aggregate()
            .Match(BuildFilter(periodStart, periodEnd))
            .Group(
                data => new DateTime(data.Timestamp.Year, data.Timestamp.Month, data.Timestamp.Day, data.Timestamp.Hour, 0, 0), 
                group => new ConsolidatedData
                {
                    Timestamp = group.Key,
                    Value = group.Max(data => data.Value)
                })
            .Sort(Builders<ConsolidatedData>.Sort.Ascending(data => data.Timestamp))
            .ToCursorAsync();
    }

    private FilterDefinition<Data> BuildFilter(DateTime? periodStart, DateTime? periodEnd)
    {
        var filterBuilder = Builders<Data>.Filter;

        var filters = new List<FilterDefinition<Data>>();
        if (periodStart != null)
        {
            filters.Add(filterBuilder.Where(data => data.Timestamp >= periodStart));
        }
        if (periodEnd != null)
        {
            filterBuilder.Where(data => data.Timestamp < periodEnd);
        }
        return filters.Any()
            ? filterBuilder.And(filters)
            : filterBuilder.Empty;
    }
}