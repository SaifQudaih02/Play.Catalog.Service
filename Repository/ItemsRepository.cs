using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repository;

public class ItemsRepository
{
    private const string collectionName = "Items";
    private readonly IMongoCollection<Item> dbCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
    
    public ItemsRepository()
    {
        var mangoClient = new MongoClient("mongodb://localhost:27017");
        var database = mangoClient.GetDatabase("Catalog");
        dbCollection = database.GetCollection<Item>(collectionName);
    }
    
    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }
    
    public async Task<Item> GetAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task CreateAsync(Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await dbCollection.InsertOneAsync(item);
    }

    public async Task UpdateAsync(Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
       
        FilterDefinition<Item> filter = filterBuilder.Eq(existionEntity => existionEntity.Id, item.Id);
        await dbCollection.ReplaceOneAsync(filter, item);
        
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(existionEntity => existionEntity.Id, id);
        await dbCollection.DeleteOneAsync(filter);  
    }
    
}