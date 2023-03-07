using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infra;
using Post.Cmd.Domain;

namespace Post.Cmd.Infra.Handlers
{

    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid Id)
        {
            PostAggregate postAggregate = new();
            var events = await _eventStore.GetEventsAsync(Id);

            if (events != null && events.Any())
            {
                postAggregate.ReplayEvents(events);
                postAggregate.Version = events.Select(x => x.Version).Max();
            }

            return postAggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregateRoot)
        {
            await _eventStore.SaveEventsAsync(aggregateRoot.Id, aggregateRoot.GetUncommitedChanges(), aggregateRoot.Version);
            aggregateRoot.MockChangesAsCommited();
        }
    }
}