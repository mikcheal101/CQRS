using CQRS.Core.Domain;
using CQRS.Core.Events;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Post.Cmd.Infra.Config;

namespace Post.Cmd.Infra.Repositories
{
    public class EventStoreRepository: IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(config.Value.Database);
            _eventStoreCollection = mongoDataBase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel eventModel)
        {
            await _eventStoreCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
        }
    }
}